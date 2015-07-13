using System;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;
using Imarda.Logging;
using ImardaBusinessBase;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		private static ErrorLogger _Log = ErrorLogger.GetLogger("Session");

		public GetItemResponse<SessionObject> GetSessionObject(SessionRequest request)
		{
			var response = new GetItemResponse<SessionObject>();
			LogonLog logonLog = new LogonLog();
			logonLog.ID = SequentialGuid.NewDbGuid();
			logonLog.ApplicationID = request.ApplicationID;
			logonLog.HostIPAddress = request.HostIPAddress;
			logonLog.FailureCode = AuthenticationResult.Undefined;
			logonLog.LoginUsername = request.Username.Truncate(50);
			try
			{
				SessionObject sessionObj = null;

				//look for session in cache, if exists return it.
				sessionObj = SessionObjectCache.Instance.GetSession(request.SessionID);
				if (sessionObj != null)
				{
					//we must clone otherwise wcf may mess up the channel
					return new GetItemResponse<SessionObject>(sessionObj.Clone());
				}

				if (sessionObj == null)
				{
					if (request.SessionID != Guid.Empty)
					{
						_Log.InfoFormat("Session {0} not in cache", request.SessionID);
						//log the session expiry - this must be a seperate log with a sepereate id  see IM-4806
						LogonLog expiryLog = new LogonLog();
						expiryLog.ID = SequentialGuid.NewDbGuid();
						expiryLog.ApplicationID = request.ApplicationID;
						expiryLog.HostIPAddress = request.HostIPAddress;
						expiryLog.FailureCode = AuthenticationResult.Undefined;
						expiryLog.LoginUsername = request.Username.Truncate(50);
						expiryLog.Logon = LogonType.SessionExpired;
						expiryLog.SessionObjectID = request.SessionID;
						expiryLog.SecurityEntityID = request.SecurityEntityID;
						//companyid and userid are unknown
						SaveLogonLog(new SaveRequest<LogonLog>(expiryLog));
					}
					SecurityEntity entity;
					bool impersonation = false;
					if (request.SecurityEntityID != Guid.Empty)
					{
						var seResponse = GetSecurityEntity(new IDRequest(request.SecurityEntityID));
						ErrorHandler.Check(seResponse);
						entity = seResponse.Item;
						impersonation = true;
                        logonLog.Logon = LogonType.Impersonation;
                        if (entity != null)
					    {
					        logonLog.FailureCode = AuthenticationResult.Success;
                            logonLog.LoginUsername = entity.LoginUsername;
                        }
					    else
					    {
					        logonLog.FailureCode = AuthenticationResult.Undefined;
                            logonLog.LoginUsername = "Unknown";
                        }
					}
					else
					{
						logonLog.FailureCode = Authenticate(request.Username, request.Password, request.Mode, request.EntityType, out entity);
						logonLog.Logon = LogonType.UserLogon;
					}
					string msg = string.Format("SecurityEntity: {0} for `{1}` -> {2}/{3}", entity, request.Username, logonLog.Logon, logonLog.FailureCode);
					_Log.Info(msg);
					response.StatusMessage = msg;

					if (entity == null)
					{
						response.Status = false;
						logonLog.FailureCode = AuthenticationResult.SecurityEntityNotFound;
						logonLog.Success = false;
						logonLog.SessionObjectID = Guid.Empty;
						logonLog.SecurityEntityID = Guid.Empty;
					}
					else 
					{
						logonLog.CompanyID = entity.CompanyID;
						logonLog.SecurityEntityID = entity.ID;
						logonLog.UserID = entity.UserID;

						if (logonLog.FailureCode == AuthenticationResult.Success)
						{
							var permissionList = GetSecurityPermissionList(request.ApplicationID, entity)
									.ConvertAll<Guid>(permission => permission.SecurityObjectID);

							// Check if this it is the Imarda Admin Console trying to log in thru the provioning service
							// in that case Flags == 2, and the IAC login security object must be linked to the security entity of the user
							if (request.Mode == LoginMode.IAC && !permissionList.Contains(AuthToken.ImardaAdminServiceLogin))
							{
								msg = string.Format("IAC login {0} failed, IAC permission for {1} not found", request.Username, entity);
								_Log.Info(msg);
								response.Status = false;
								response.StatusMessage = msg;
								logonLog.FailureCode = AuthenticationResult.IACPermissionNotFound;
								logonLog.Success = false;
								logonLog.SessionObjectID = Guid.Empty;
								SaveLogonLog(new SaveRequest<LogonLog>(logonLog));
								return response;
							}

							sessionObj = new SessionObject
							{
								ApplicationID = request.ApplicationID,
								SessionID = Guid.NewGuid(),
								CRMID = entity.CRMId,
								SecurityEntityID = entity.ID,
								CompanyID = entity.CompanyID,
								Username = entity.LoginUsername,
								Password = entity.LoginPassword,
								PermissionsList = permissionList,
								Impersonation = impersonation,
								TimeZoneKey = entity.TimeZone,
								EntityName = entity.EntityName,
                                EntityType = entity.EntityType,
								EnableTimeZoneSelect = entity.EnableTimeZoneSelect,
							};
							logonLog.Success = true;
							logonLog.SessionObjectID = sessionObj.SessionID;
							SessionObjectCache.Instance.StoreSession(sessionObj);
							_Log.InfoFormat("Store new session: {0}", sessionObj);
						}
					}
					SaveLogonLog(new SaveRequest<LogonLog>(logonLog));
				}
				return new GetItemResponse<SessionObject>(sessionObj) {ErrorCode = logonLog.FailureCode.ToString()}; // StatusMessage = response.StatusMessage};
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SessionObject>>(ex);
			}
		}

		private int Authenticate(string username, string password, LoginMode loginMode, int entityType, out SecurityEntity entity)
		{
			var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
			using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityEntityByLoginUserName", username,entityType))
			{
				if (dr.Read())
				{
					entity = GetFromData<SecurityEntity>(dr);
					//handle loginEnabled
					if (!entity.LoginEnabled && loginMode != LoginMode.IAC)
					{
						_Log.InfoFormat("User {0} is not login enabled", username);
						return AuthenticationResult.LoginDisabled;
					}
					else
					{
						var req = new SaveRequest<SecurityEntity>(entity);

						if (entity.Salt != Guid.Empty)
						{
							// Check new style password hash
							string storedHash = entity.LoginPassword;
							string calculatedHash = Convert.ToBase64String(AuthenticationHelper.ComputePasswordHash(entity.Salt, password));
							if (storedHash != calculatedHash)
							{
								return AuthenticationResult.WrongPassword;
							}
							//else success
						}
						else
						{
							// Check old style password hash and upgrade to new style
							string saltyPassword = AuthenticationHelper.ComputeHashOldStyle(username, password);
							if (entity.LoginPassword != saltyPassword)
							{
								return AuthenticationResult.WrongPassword;
							}
							else
							{
								// upgrade
								req.SetFlag("UpdatePassword", true);
								entity.LoginPassword = password; // tricky: plain text will be hashed in SaveUserSecurityEntity
								// success
							}
						}
						entity.LastLogonDate = DateTime.UtcNow;
						SaveUserSecurityEntity(req);
						return AuthenticationResult.Success;
					}
				}
				entity = null;
				return AuthenticationResult.SecurityEntityNotFound;
			}
		}

	}
}
