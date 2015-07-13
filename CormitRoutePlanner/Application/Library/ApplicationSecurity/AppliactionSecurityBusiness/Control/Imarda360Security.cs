
using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda360.Infrastructure.ConfigurationService;
using Imarda360Application.Task;
using Imarda360Base;
using ImardaConfigurationBusiness;
using ImardaSecurityBusiness;
using Imarda.Lib;
using System.Linq;

using System.Text;
using ImardaBusinessBase;

namespace Imarda360Application.Security
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaSecurity : FernBusinessBase.BusinessBase, IImardaSecurity
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "I360 Security Application";


		public BusinessMessageResponse IsAuthenticated(SessionRequest request)
		{
			throw new NotImplementedException(); // does not need to be implemented, only required by IImardaSecurity
		}

		public GetItemResponse<ConfiguredSessionObject> GetSessionByID(SessionRequest request)
		{
			throw new NotImplementedException(); // does not need to be implemented, only required by IImardaSecurity
		}

		#region Login
		public GetItemResponse<ConfiguredSessionObject> Login(SessionRequest request)
		{
			try
			{
				SessionObject session;
				if (request.Username != null)
				{
					if (request.SessionID != Guid.Empty)
					{
						Logout(request);
						request.SessionID = Guid.Empty;
					}
					// this clears old session id if user name is used to log in otherwise we are logging in with somebody else's old session id!!
					session = null;
				}
				else
				{
					session = SessionObjectCache.Instance.GetSession(request.SessionID);
				}

				if (session == null)
				{
					GetItemResponse<SessionObject> resp1 = null;
					var service0 = ImardaProxyManager.Instance.IImardaSecurityProxy;
					ChannelInvoker.Invoke(delegate(out IClientChannel channel)
					{
						channel = service0 as IClientChannel;
						resp1 = service0.GetSessionObject(request);
						if (request.Mode == LoginMode.Normal) ErrorHandler.Check(resp1);
						session = resp1.Item;
					});
					if (session == null)
					{
						var statusMessage = "Authentication failed";
						if (resp1 != null)
						{
							if (resp1.ErrorCode == "-1")
								statusMessage = "Invalid user";
							if (resp1.ErrorCode == "0")
								statusMessage = "Success";
							else if (resp1.ErrorCode == "1")
								statusMessage = "Invalid username";
							else if (resp1.ErrorCode == "2")
								statusMessage = "User has no login permission";
							if (resp1.ErrorCode == "3")
								statusMessage = "Login is disabled";
							else if (resp1.ErrorCode == "4")
								statusMessage = "Invalid password";
						}
						return new GetItemResponse<ConfiguredSessionObject> { Status = false, StatusMessage = statusMessage, ErrorCode = resp1.ErrorCode };
					}
				}
				else
				{
					var service1 = ImardaProxyManager.Instance.IImardaSecurityProxy;
					ChannelInvoker.Invoke(delegate(out IClientChannel channel3)
					{
						channel3 = service1 as IClientChannel;
						var resp3 = service1.GetSecurityEntity(new IDRequest(session.SecurityEntityID));
						SecurityEntity se = resp3.Item;
						session.TimeZoneKey = se.TimeZone;
					});
				}

				var config = new SessionConfigGroup();

				var service2 = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ConfiguredSessionObject cfgSession = null;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel2)
				{
					channel2 = service2 as IClientChannel;
					Guid[] ids = ConfigGroup.GetIDs(config);
					var request2 = new ConfigListRequest(ids, session.CompanyID, session.CRMID);
					service2.RemoveFromCache(request2);
					var resp2b = service2.GetConfigValueList(request2);
					if (request.Mode == LoginMode.Normal) ErrorHandler.Check(resp2b);
					ConfigValue[] values = resp2b.List.ToArray();
					ConfigGroup.SetValues(config, values);

					cfgSession = new ConfiguredSessionObject(session, config, request.Mode);
					config.PreferredMeasurementUnits = CultureHelper.CalcPreferences(config, service2);

					var cacheSession = cfgSession.StripConfig(request.Mode);
					SessionObjectCache.Instance.StoreSession(cacheSession);
				});

				var sb = new StringBuilder();
				sb.AppendKV("User", cfgSession.Username)
					.AppendKV("AppID", cfgSession.ApplicationID.ToString().ToUpperInvariant())
					.AppendKV("Locale", cfgSession.PreferredCulture) // used in formatter for notification templates!
					.AppendKV("Region", cfgSession.Configuration.Region)
					.AppendKV("Impers", cfgSession.Impersonation)
					.AppendKV("EvTime", DateTime.UtcNow, "~");
				var logonEventID = new Guid("4c2f21cb-fcdd-4d6b-a6bd-a928680bee05");

				AlertTaskHelper.SaveAlertTask(
					cfgSession.CompanyID,
					logonEventID,
					cfgSession.CRMID,
					cfgSession.CRMID,
					cfgSession.Username,
					Guid.Empty,
					sb,
					TimeZoneInfo.FindSystemTimeZoneById(cfgSession.TimeZoneKey),
					Guid.Empty);

				cfgSession.Password = null; // clear password hash before returning
				return new GetItemResponse<ConfiguredSessionObject>
				{
					Item = cfgSession,
					Status = true
				};
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ConfiguredSessionObject>>(ex);
			}
		}
		#endregion

		#region Logout
		public BusinessMessageResponse Logout(SessionRequest request)
		{
			try
			{
				var session = SessionObjectCache.Instance.GetSession(request.SessionID);
				if (session == null)
				{
					return new BusinessMessageResponse();
				}
				var resp = new BusinessMessageResponse();
				var service2 = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel2)
															{
																channel2 = service2 as IClientChannel;
																var request2 = new ConfigListRequest(null, session.CompanyID, session.CRMID);
																resp = service2.RemoveFromCache(request2);
															});
				SessionObjectCache.Instance.DeleteSession(request.SessionID);
				//save logonlog
				if (resp.Status)
				{
					var service3 = ImardaProxyManager.Instance.IImardaSecurityProxy;
					ChannelInvoker.Invoke(delegate(out IClientChannel channel3)
					{
						channel3 = service3 as IClientChannel;
						LogonLog logonLog = new LogonLog();
						logonLog.ID = SequentialGuid.NewDbGuid();
						logonLog.ApplicationID = request.ApplicationID;
						logonLog.HostIPAddress = request.HostIPAddress;
						logonLog.CompanyID = session.CompanyID;
						logonLog.SecurityEntityID = session.SecurityEntityID;
						logonLog.SessionObjectID = session.SessionID;
						logonLog.LoginUsername = session.Username;
						logonLog.UserID = session.CRMID;
						logonLog.Logon = (request.Username == null) ? LogonType.UserLogoff : LogonType.AutoLogoff;
						var request3 = new SaveRequest<LogonLog>(logonLog);
						resp = service3.SaveLogonLog(request3);
					});
				}
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion

		#region ResetPassword
		public BusinessMessageResponse ResetPassword(ResetPasswordRequest request)
		{
			var response = new BusinessMessageResponse();
			string username = request.Username;
			string email = request.Email;

			try
			{
				//TODO: verify the username against security service, reset password
			}
			catch
			{
				response.Status = false;
				response.StatusMessage = "The user name " + username + " is invalid";
			}

			try
			{
				//TODO: send new password to the email address above
			}
			catch
			{
				if (response.Status)
				{
					response.Status = false;
					response.StatusMessage = "Fail to send password to " + email;
				}
				else
				{
					response.StatusMessage += "\n Fail to send password to " + email;
				}
			}

			return response;
		}
		#endregion

		#region GetSession
		//Use this call if it is expected that a session exists in cache, session is looked up by either SessionID or AccessToken
		public GetItemResponse<ConfiguredSessionObject> GetSession(SessionRequest request)
		{
			try
			{
				SessionObject session = null;
				if (request.SessionID != Guid.Empty)
				{
					session = SessionObjectCache.Instance.GetSession(request.SessionID);
				}
				//if (session == null && !string.IsNullOrWhiteSpace(request.AccessToken))
				//{
				//	session = SessionObjectCache.Instance.FindSession(request.AccessToken);
				//}

				if (session == null)
				{
					GetItemResponse<SessionObject> resp1 = null;
					var service0 = ImardaProxyManager.Instance.IImardaSecurityProxy;
					ChannelInvoker.Invoke(delegate(out IClientChannel channel)
					{
						channel = service0 as IClientChannel;
						resp1 = service0.GetSessionObject(request);
						if (request.Mode == LoginMode.Normal) ErrorHandler.Check(resp1);
						session = resp1.Item;
					});
					if (session == null)
					{
						var statusMessage = "Authentication failed";
						if (resp1 != null)
						{
							if (resp1.ErrorCode == "-1")
								statusMessage = "Invalid user";
							if (resp1.ErrorCode == "0")
								statusMessage = "Success";
							else if (resp1.ErrorCode == "1")
								statusMessage = "Invalid username";
							else if (resp1.ErrorCode == "2")
								statusMessage = "User has no login permission";
							if (resp1.ErrorCode == "3")
								statusMessage = "Login is disabled";
							else if (resp1.ErrorCode == "4")
								statusMessage = "Invalid password";
						}
						return new GetItemResponse<ConfiguredSessionObject> { Status = false, StatusMessage = statusMessage, ErrorCode = resp1.ErrorCode };
					}
				}
				else
				{
					var service1 = ImardaProxyManager.Instance.IImardaSecurityProxy;
					ChannelInvoker.Invoke(delegate(out IClientChannel channel3)
					{
						channel3 = service1 as IClientChannel;
						var resp3 = service1.GetSecurityEntity(new IDRequest(session.SecurityEntityID));
						SecurityEntity se = resp3.Item;
						session.TimeZoneKey = se.TimeZone;
					});
				}

				var config = new SessionConfigGroup();

				var service2 = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ConfiguredSessionObject cfgSession = null;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel2)
				{
					channel2 = service2 as IClientChannel;
					Guid[] ids = ConfigGroup.GetIDs(config);
					var request2 = new ConfigListRequest(ids, session.CompanyID, session.CRMID);
					service2.RemoveFromCache(request2);
					var resp2b = service2.GetConfigValueList(request2);
					if (request.Mode == LoginMode.Normal) ErrorHandler.Check(resp2b);
					ConfigValue[] values = resp2b.List.ToArray();
					ConfigGroup.SetValues(config, values);

					cfgSession = new ConfiguredSessionObject(session, config, request.Mode);
					config.PreferredMeasurementUnits = CultureHelper.CalcPreferences(config, service2);

					var cacheSession = cfgSession.StripConfig(request.Mode);
					SessionObjectCache.Instance.StoreSession(cacheSession);
				});

				var sb = new StringBuilder();
				sb.AppendKV("User", cfgSession.Username)
					.AppendKV("AppID", cfgSession.ApplicationID.ToString().ToUpperInvariant())
					.AppendKV("Locale", cfgSession.PreferredCulture) // used in formatter for notification templates!
					.AppendKV("Region", cfgSession.Configuration.Region)
					.AppendKV("Impers", cfgSession.Impersonation)
					.AppendKV("EvTime", DateTime.UtcNow, "~");
				var logonEventID = new Guid("4c2f21cb-fcdd-4d6b-a6bd-a928680bee05");

				AlertTaskHelper.SaveAlertTask(
					cfgSession.CompanyID,
					logonEventID,
					cfgSession.CRMID,
					cfgSession.CRMID,
					cfgSession.Username,
					Guid.Empty,
					sb,
					TimeZoneInfo.FindSystemTimeZoneById(cfgSession.TimeZoneKey),
					Guid.Empty);

				cfgSession.Password = null; // clear password hash before returning
				return new GetItemResponse<ConfiguredSessionObject>
				{
					Item = cfgSession,
					Status = true
				};
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ConfiguredSessionObject>>(ex);
			}
		}
		#endregion

		public BusinessMessageResponse SetAcessTokenOnSession(SessionRequest request)
		{
			try
			{
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					//response = service.SetDeletedSecurityEntityByCRMID(request);
				});
				return null; // response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


		#region GetUserSecurityEntity
		public GetItemResponse<SecurityEntity> GetUserSecurityEntity(IDRequest request)
		{
			try
			{
				GetItemResponse<SecurityEntity> response = null;
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetSecurityEntityByCrmID(request);
				});
				var cfgservice = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = cfgservice as IClientChannel;
					var localeID = new Guid("5e93c6a7-3ee3-4a75-a6f8-b783296afa7d");
					var cfgreq = new ConfigRequest(localeID, null, request.CompanyID, request.ID) { IgnoreCache = true };
					var cfgresp = cfgservice.GetConfigValue(cfgreq);
					ErrorHandler.Check(cfgresp);
					response.Item.Locale = cfgresp.Item.As<string>();
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
		}
		#endregion
		#region SaveUserSecurityEntity
		public BusinessMessageResponse SaveUserSecurityEntity(SaveRequest<SecurityEntity> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveUserSecurityEntity(request);
					ErrorHandler.Check(response);
				});
				var cfgservice = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = cfgservice as IClientChannel;
					SecurityEntity se = request.Item;
					var localeID = new Guid("5e93c6a7-3ee3-4a75-a6f8-b783296afa7d");
					var cfgreq = new ConfigRequest(localeID, se.Locale, se.CompanyID, se.CRMId) { IgnoreCache = true };
					var cfgresp = cfgservice.SetUserLocale(cfgreq);
					ErrorHandler.Check(cfgresp);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region SecurityObjects For Entity
		#region Get Assigned SecurityObjects

		public GetListResponse<SecurityObject> GetAssignedSecurityObjects(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<SecurityObject>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetAssignedSecurityObjects(request);
					RemoveUnownedSecurityObjects(service, request, response);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		#endregion
		#region Get UnAssigned SecurityObjects

		/// <summary>
		/// Get list of unassigned security objects, but remove objects that the caller does not have.
		/// </summary>
		/// <param name="request"></param>
		/// <returns>unassigned security objects, always subset of caller's rights</returns>
		public GetListResponse<SecurityObject> GetUnassignedSecurityObjects(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<SecurityObject>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetUnassignedSecurityObjects(request);
					RemoveUnownedSecurityObjects(service, request, response);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		private static readonly Guid APIAccessPermission = new Guid("2215F16E-7189-48CE-BACA-068721EE7AF4");
		private static readonly Guid ImardaCompany  = new Guid("78c46d66-b886-44d0-a3c2-3aa9b12c4d98");

		private void RemoveUnownedSecurityObjects(
			ImardaSecurityBusiness.IImardaSecurity service,
			IDRequest request,
			GetListResponse<SecurityObject> response)
		{
			if (ServiceMessageHelper.IsSuccess(response))
			{
				Guid callerSecurityEntityID;
				if (request.Get<Guid>("UserID", out callerSecurityEntityID))
				{
					GetListResponse<SecurityObject> resp1 = service.GetKnownSecurityObjects(new IDRequest(callerSecurityEntityID));
					ErrorHandler.Check(resp1);
					if (resp1.List != null)
					{
						List<Guid> maxlist = resp1.List.Select(known => known.ID).ToList();
						response.List.RemoveAll(so => !maxlist.Contains(so.ID));
					}
				}

				//Include APIAccessPermission only for companies
				if (request.Get("Owner", "User") == "User")
					response.List.Remove(response.List.Find(so => so.ID == APIAccessPermission));
			}
		}
		#endregion


		#region Get Assigned and UnAssigned SecurityObjects
		public GenericList360Response<GetListResponse<SecurityObject>> GetAssignedAndUnAssignedSecurityObjects(IDRequest request)
		{
			GenericList360Response<GetListResponse<SecurityObject>> response = new GenericList360Response<GetListResponse<SecurityObject>>();
			GetListResponse<SecurityObject> assigned = GetAssignedSecurityObjects(request);
			if (assigned != null)
				assigned.StatusMessage = "Assigned";
			GetListResponse<SecurityObject> notAssigned = GetUnassignedSecurityObjects(request);
			if (notAssigned != null)
				notAssigned.StatusMessage = "notAssigned";
			response.List.Add(assigned);
			response.List.Add(notAssigned);
			return response;
		}
		#endregion


		#region Assign SecurityObjects To Entity

		/// <summary>
		/// This is the default usage and has been changed to only affect I360 permissions and ignore IAC permissions
		/// </summary>
		/// <param name="securityObjects"></param>
		/// <returns></returns>
		public SolutionMessageResponse AssignSecurityObjectsToEntity(SaveListRequest<SecurityObject> securityObjects)
		{
			return AssignSecurityObjectsToEntityGeneric(securityObjects);
		}

		/// <summary>
		/// This method will only affect IAC permissions and ignore I360 permissions
		/// </summary>
		/// <param name="securityObjects"></param>
		/// <returns></returns>
		public SolutionMessageResponse AssignSecurityObjectsToEntityForIac(SaveListRequest<SecurityObject> securityObjects)
		{
			return AssignSecurityObjectsToEntityGeneric(securityObjects, false);
		}

		private SolutionMessageResponse AssignSecurityObjectsToEntityGeneric(SaveListRequest<SecurityObject> securityObjects, bool assignI360 = true)
		{

		/*							permissions
			 * target user has		  : A B C		list1
			 * editing user has		 :   B C D E	list2
			 * param 'securityObjects'  :	 C D	  list3 (must be subset of list2)
			 * ------------------------------------
			 * to be deleted			:   B		  list4
			 * to be added			  :	   D	  list5
			 * target user has now	  : A   C D  
			 * 
			 * Algorithm: 
			 * foreach p in list2
			 *   if p in list1 and p not in list3 then add to list4
			 *   else if p not in list1 and p in list3 then add to list5
			 */

			Guid targetID = securityObjects.Get("TargetID", Guid.Empty);
			Guid editorID = securityObjects.Get("EditorID", Guid.Empty);
			Guid appID = new Guid("B9E34B8D-F105-4E21-AFED-60F8500B9EDB");

			if (targetID == Guid.Empty || editorID == Guid.Empty)
			{
				throw new ArgumentException("Required params TargetID and EditorID");
			}

			//add back APIAccessPermission for Imarda user in case it was removed for display
			if (targetID == ImardaCompany && securityObjects.List.Find(so => so.ID == APIAccessPermission) == null)
				securityObjects.List.Add(new SecurityObject() {ID = APIAccessPermission});
			

			ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;

				//retrieve existing target user list:
				var req1 = new IDRequest(targetID, "appid", appID);
				
				//PG20140205 - Original code was retrieving all entities I360 + IAC
				//var resp1 = service.GetEntitySecurityEntryList(req1);
				//PG20140205 - New code retrieves I360 OR IAC
				var resp1 = assignI360 ? service.GetEntitySecurityEntryListForI360(req1) : service.GetEntitySecurityEntryListForIac(req1);

				ErrorHandler.Check(resp1);
				Guid[] list1 = resp1.List.Select(se => se.SecurityObjectID).ToArray();

				//retrieve list of editing user:
				var req2 = new IDRequest(editorID, "appid", appID);
				
				//PG20140205 - Original code was retrieving all entities I360 + IAC
				//var resp2 = service.GetEntitySecurityEntryList(req2);
				//PG20140205 - New code retrieves I360 OR IAC
				var resp2 = assignI360 ? service.GetEntitySecurityEntryListForI360(req2) : service.GetEntitySecurityEntryListForIac(req2);

				ErrorHandler.Check(resp2);
				Guid[] list2 = resp2.List.Select(se => se.SecurityObjectID).ToArray();

				//input parameter contains assigned permissions
				Guid[] list3 = securityObjects.List.Select(so => so.ID).ToArray();

				var list4 = new List<SecurityEntryKey>(); // delete
				var list5 = new List<SecurityEntry>(); // create

				foreach (Guid id in list2)
				{
					if (list1.Contains(id) && !list3.Contains(id))
					{
						// delete
						var se = new SecurityEntryKey { EntityID = targetID, SecurityObjectID = id };
						list4.Add(se);
					}
					else if (!list1.Contains(id) && list3.Contains(id))
					{
						// create
						var se = new SecurityEntry();
						se.ID = SequentialGuid.NewDbGuid();
						se.EntityID = targetID;
						se.SecurityObjectID = id;
						se.EntryType = 0;
						se.UserID = editorID;
						se.ApplicationID = appID;
						list5.Add(se);
					}
				}
				var delReq = new SaveListRequest<SecurityEntryKey>(list4);
				delReq.Put("UserID", editorID);
				var resp4 = service.DeleteSecurityEntryList(delReq);
				ErrorHandler.Check(resp4);

				var createReq = new SaveListRequest<SecurityEntry>(list5);
				//editorID included in each SecurityEntry
				var resp5 = service.SaveSecurityEntryList(createReq);
				ErrorHandler.Check(resp5);
			});
			return new SolutionMessageResponse();
		}
		#endregion
		#endregion

		#region GetSecurityEntityByLoginUserName
		public GetItemResponse<SecurityEntity> GetSecurityEntityByLoginUserName(IDRequest request)
		{
			try
			{
				GetItemResponse<SecurityEntity> response = null;
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetSecurityEntityByLoginUserName(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
		}
		#endregion

		public BusinessMessageResponse SetDeletedSecurityEntityByCRMID(IDRequest request)
		{
			try
			{
				BusinessMessageResponse response = null;
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SetDeletedSecurityEntityByCRMID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
		}

		public GetListResponse<LogonLog> GetTopNLogonLogListBySecurityEntityID(IDRequest request)
		{
			try
			{
				GetListResponse<LogonLog> response = null;
				ImardaSecurityBusiness.IImardaSecurity service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetTopNLogonLogListBySecurityEntityID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<LogonLog>>(ex);
			}
		}

		#region SecurityObjects for Profile

		#region GetSecurityObjectsByAreaType = all for AreaType
		public GetListResponse<SecurityObject> GetSecurityObjectsByApplicationID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<SecurityObject>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetSecurityObjectsByApplicationID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion

		#region GetSecurityObjectsAssignedByAreaType = assigned to request.ID for AreaType
		public GetListResponse<SecurityObject> GetSecurityObjectsAssignedByAreaType(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<SecurityObject>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetSecurityObjectsAssignedByApplicationID(request);
					//RemoveUnownedSecurityObjects(service, request, response);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion

		#endregion

	}
}

