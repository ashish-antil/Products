using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using System.Security;
using System.Globalization;
using Imarda.Lib;


namespace ImardaSecurityBusiness
{
	public class SessionObjectCache
	{

		#region Singleton
		private readonly Guid SpecialSessionID = new Guid("FF30ADA4-E2A3-482D-9098-9ECB595CEEA5");

		static SessionObjectCache()
		{
		}


		private static readonly SessionObjectCache _Instance = new SessionObjectCache();

		public static SessionObjectCache Instance { get { return _Instance; } }

		private SessionObjectCache()
		{

			_Impl = BusinessObjectCache<SessionObject>.Instance;
			CacheID = _Impl.GetHashCode();
		}
		#endregion

		public readonly int CacheID;

		private BusinessObjectCache<SessionObject> _Impl;

		public TimeSpan Expiration
		{
			get { return _Impl.Expiration; }
			set { _Impl.Expiration = value; }
		}

		/// <summary>
		/// Get an existing session from the cache or return null if note found.
		/// </summary>
		/// <param name="sessionID">session to look up, passing Guid.Emtpy is not allowed</param>
		/// <returns></returns>
		public SessionObject GetSession(Guid sessionID)
		{
			SessionObject sessionObj = null;

			if (_Impl != null)
			{
				sessionObj = _Impl[sessionID.ToString()];
			}
			return sessionObj;
		}

		/// <summary>
		/// Store a session in the cache.
		/// </summary>
		/// <param name="sessionObj">session to store, ID must not be Guid.Empty</param>
		public void StoreSession(SessionObject sessionObj)
		{
			if (sessionObj.SessionID == Guid.Empty) throw new ArgumentException("StoreSession: SessionID must not be empty");
			if (_Impl != null)
			{
				_Impl[sessionObj.SessionID.ToString()] = sessionObj;
			}
		}

		/// <summary>
		/// Remove the session object.
		/// </summary>
		/// <param name="sessionID"></param>
		public void DeleteSession(Guid sessionID)
		{
			if (_Impl != null) _Impl[sessionID.ToString()] = null;
		}

		public SessionObject CheckPermissions(GenericRequest request, Guid authToken)
		{
			var session = CheckPermissions(request.SessionID, authToken);
			return session;
		}

		public SessionObject CheckPermissions(ParameterMessageBase request, Guid authToken)
		{
			var session = CheckPermissions(request.SessionID, authToken); 
			if (session != null)
			{
				if (session.CompanyID != Guid.Empty && request.CompanyID != FullBusinessEntity.SystemGuid && request.CompanyID == Guid.Empty)
				{
					request.CompanyID = session.CompanyID;
				}
				if (session.CRMID != Guid.Empty)
				{
					request.UserID = session.CRMID;
				}
			}
			return session;
		}

		/// <summary>
		/// Check whether the authToken is in the permissions list. Throw SecurityException 
		/// if unauthorized.
		/// </summary>
		/// <param name="sessionID">identifies session that contains the permissions list</param>
		/// <param name="authToken">token representing a requested permission</param>
		/// <returns>the SessionObject that has the sessionID, if authorized, otherwise null.</returns>
		public SessionObject CheckPermissions(Guid sessionID, Guid authToken)
		{
			var session = GetSession(sessionID);
			if (session != null) return session;

			// session not in cache
			if (sessionID == SpecialSessionID) //HACK for Unit API
			{
				session = new SessionObject { SessionID = SpecialSessionID, TimeZoneKey = "UTC" };
				StoreSession(session);
			}
			else
			{
				throw new SessionException(string.Format("NO_SESSION|sessionID:{0}", sessionID));
			}

			//if (session == null)
			//{
			//  //TODO not in cache => load it and put in cache
			//}
			//if (!session.GrantedPermissions.Contains(authToken))
			//{
			//  throw new SessionException("PERMISSION_DENIED|" + authToken);
			//}
			return session;
		}
	}

	public class SessionException : Exception
	{
		public SessionException()
		{
		}

		public SessionException(string msg)
			: base(msg)
		{
		}

		public SessionException(string msg, Exception inner)
			: base(msg, inner)
		{
		}
	}
}
