#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Imarda.Lib;
using ImardaBusinessBase;
using ImardaSecurityBusiness;

#endregion

// ReSharper disable once CheckNamespace
namespace Cormit.Application.RouteApplication.Security
{
    /// <summary>
    ///     Session object in the application layer. This derives from the business session object
    ///     and adds a configuration.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ConfiguredSessionObject : SessionObject
    {
        private SessionConfigGroup _configuration;
        private CultureInfo _culture;
        private ImardaFormatProvider _formatProvider;
        private HashSet<Guid> _grantedPermissions;
        private string _preferredMeasurementUnits;
        private TimeZoneInfo _timeZone;

		private string _accessToken;

        public ConfiguredSessionObject()
        {
        }

        public ConfiguredSessionObject(SessionObject session, SessionConfigGroup userCfg, LoginMode mode)
        {
            ApplicationID = session.ApplicationID;
            SessionID = session.SessionID;
            CRMID = session.CRMID;
            SecurityEntityID = session.SecurityEntityID;
            CompanyID = session.CompanyID;
            Username = session.Username;
            Password = session.Password;
            Impersonation = session.Impersonation;
            PermissionsList = session.PermissionsList;
            Locale = userCfg.Locale;
            EntityName = session.EntityName;
            EntityType = session.EntityType;
            TimeZoneKey = session.TimeZoneKey ?? "UTC";
            EnableTimeZoneSelect = session.EnableTimeZoneSelect;
            _configuration = userCfg;
            Initialize(mode);
        }

        [DataMember]
        public string[] OwnedTrackIDs { private set; get; }

        [DataMember]
        public Guid[] OwnedUnitIDs { private set; get; }

        public string PreferredMeasurementUnits
        {
            get { return _preferredMeasurementUnits; }
        }


        public override ImardaFormatProvider FormatProvider
        {
            get { return _formatProvider; }
        }

        public HashSet<Guid> GrantedPermissions
        {
            get
            {
                if (_grantedPermissions == null)
                {
                    _grantedPermissions = new HashSet<Guid>(PermissionsList);
                }
                return _grantedPermissions;
            }
        }

        [DataMember]
        public SessionConfigGroup Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        [DataMember]
        public bool RequiresTimeZoneConversion { get; set; }

        [DataMember]
        public string Locale { get; set; }

        public CultureInfo PreferredCulture
        {
            get
            {
                if (_culture == null)
                {
                    _culture = new CultureInfo(Locale, false);
                }
                return _culture;
            }
        }

        #region Time Zones

        public TimeZoneInfo PreferredZone
        {
            get
            {
                if (_timeZone == null)
                {
                    if (string.IsNullOrEmpty(TimeZoneKey))
                    {
                        TimeZoneKey = TimeZoneInfo.Local.Id;
                    }
                    _timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneKey);
                }
                return _timeZone;
            }
            set { _timeZone = value; }
        }

        /// <summary>
        ///     Converts from UTC based time to preferred time zone
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public DateTime FromUtc(DateTime utc)
        {
            if (utc == DateTime.MinValue) return utc;
            if (utc.Kind != DateTimeKind.Utc) throw new ArgumentException("Time must be UTC");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, PreferredZone); // DateTimeKind.Unspecified is returned
        }

        /// <summary>
        ///     Converts from configured date time to UTC. The DateTimeKind is expected to be Unspecified.
        /// </summary>
        /// <param name="pdt">time in preferred time zone</param>
        /// <returns>UTC based time</returns>
        public DateTime ToUtc(DateTime pdt)
        {
            if (pdt == DateTime.MinValue) return pdt;
            if (pdt.Kind != DateTimeKind.Unspecified) throw new ArgumentException("Time must be 'Unspecified'");
            return TimeZoneInfo.ConvertTimeToUtc(pdt, PreferredZone); // DateTimeKind.Utc is returned
        }

        #endregion

        public void SetOwnedUnits(string[] trackIDs, Guid[] guids)
        {
            OwnedTrackIDs = trackIDs;
            OwnedUnitIDs = guids;
        }


        public ConfiguredSessionObject StripConfig(LoginMode mode)
        {
            var session = new ConfiguredSessionObject(this, _configuration, mode);
            session.Configuration = null;
            return session;
        }


		[DataMember]
		public string AccessToken
		{
			get { return _accessToken; }
			set { _accessToken = value; }
		}

        /// <summary>
        ///     Further initialize this object.
        ///     Pull information out of the SessionConfigGroup eligible for caching into this ConfiguredSessionObject,
        ///     because the SessionConfigGroup is going to be removed before caching.
        /// </summary>
        public void Initialize(LoginMode mode = LoginMode.Normal)
        {
            var dfltFormatProvider = PreferredCulture.IsNeutralCulture ? new CultureInfo("en-NZ") : PreferredCulture;
            var measurementFormatProvider = new MeasurementFormatInfo(dfltFormatProvider.NumberFormat);
            _preferredMeasurementUnits = _configuration.PreferredMeasurementUnits;
            IDictionary map = _preferredMeasurementUnits.KeyValueMap(ValueFormat.Strings, true);
            measurementFormatProvider.SetPreferences(map);
            _formatProvider = new ImardaFormatProvider(dfltFormatProvider, measurementFormatProvider, PreferredZone);

            if (mode != LoginMode.Mobile
                && mode != LoginMode.IAC)
            {
                RequiresTimeZoneConversion = true;
            }
        }

        public override string ToString()
        {
            return string.Format("Session({0} {1} {2})", SessionID, _culture.Name, _timeZone);
        }
    }
}