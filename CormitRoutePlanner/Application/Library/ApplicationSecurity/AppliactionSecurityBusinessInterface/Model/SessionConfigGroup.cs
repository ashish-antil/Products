using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Imarda360.Infrastructure.ConfigurationService;
using Imarda.Lib;
using System.Collections;
using ImardaConfigurationBusiness;


namespace Imarda360Application.Security
{
	/// <summary>
	/// Contains user configuration settings for storage in the session object.
	/// The configuration hierarchy levels expected are:
	/// LEVEL 1 : Company ID
	/// LEVEL 2 : Person ID or the user who logs in.
	/// </summary>
	[DataContract]
	public class SessionConfigGroup : ICultureConfigGroup
	{
		//TIP code snippets: cfga (simple), cfgat (template). Ask MV


		
		#region ToolTip
		[DataMember]
		[ConfigAssign("69152DBA-9140-49F0-8EAB-D9B7CFD1CD3F", "<html>Missing 69152DBA</html>")]
		public string ToolTipHtml;

		private ConfigTemplate _ToolTipTemplate;

		public ConfigTemplate ToolTipTemplate
		{
			get
			{
				if (_ToolTipTemplate == null)
				{
					_ToolTipTemplate = new ConfigTemplate(ToolTipHtml);
				}
				return _ToolTipTemplate;
			}
		}
		#endregion


		#region Culture Settings
		[DataMember]
		[ConfigAssign("5E93C6A7-3EE3-4A75-A6F8-B783296AFA7D", "en-NZ")]
		public string _Locale;

		public string Locale { get { return _Locale; } } // required by interface.

		[DataMember]
		[ConfigAssign("A7A52B80-BC9F-4FA5-9E31-8A7B21783736", "3A5ED551-E429-4754-8727-7EA566DD402C")]
		public string StrUnitSystemID;

		/// <summary>
		/// Use this ID to identify level 1 in the national culture hierarchy in the configuration table.
		/// </summary>
		public Guid UnitSystem
		{
			get { return string.IsNullOrEmpty(StrUnitSystemID.Trim()) ? Guid.Empty : new Guid(StrUnitSystemID); }
		}

        //IM-4720
        [DataMember]
        [ConfigAssign("7d78f244-d825-4981-b1a2-a746db00a178", "00000000-0000-0000-0000-000000000000")]
        public string StrDefaultHomePage;
        public Guid DefaultHomePage
        {
            get { return string.IsNullOrEmpty(StrDefaultHomePage.Trim()) ? Guid.Empty : new Guid(StrDefaultHomePage); }
        }

		/// <summary>
		/// Get the part of the locale after the first dash, e.g. "en-NZ"=>"NZ"
		/// </summary>
		public string Region
		{
			get
			{
				if (string.IsNullOrEmpty(Locale)) return null;
				int p = Locale.IndexOf('-');
				return p == -1 ? null : Locale.Substring(p + 1);
			}
		}


		/// <summary>
		/// Will get merged with item C2F6EC0D-E1CB-4978-9C27-DEF434ED48B2"
		/// overriding any values with the same key.
		/// </summary>
		[DataMember]
		[ConfigAssign("F59CCF57-588F-4467-AF61-E4E1A8D3E32D", "")]
		public string _PreferredMeasurementUnits;

		public string PreferredMeasurementUnits
		{
			get { return _PreferredMeasurementUnits; }  // required by interface
			set { _PreferredMeasurementUnits = value; }
		}

    
		#endregion Culture Settings

		#region VehicleDetails
		[DataMember]
		[ConfigAssign("350AEC9E-0E77-46C1-9764-AD2C99DAF67B", "<html>Missing 350AEC9E</html>")]
		public string VehicleDetailsSummaryHtml;

		private ConfigTemplate _VehicleDetailsSummaryTemplate;

		public ConfigTemplate VehicleDetailsSummaryTemplate
		{
			get
			{
				if (_VehicleDetailsSummaryTemplate == null)
				{
					_VehicleDetailsSummaryTemplate = new ConfigTemplate(VehicleDetailsSummaryHtml);
				}
				return _VehicleDetailsSummaryTemplate;
			}
		}
		#endregion

		#region VehicleEdit
		//*depercated: replaced by '/VehicleManager/html/vehicleEditForm.htm'*/
		[DataMember]
		[ConfigAssign("954C2A71-908F-4863-A884-522E2A35DD34", "<html>Missing 954C2A71</html>")]
		public string VehicleEditHtml;

		private ConfigTemplate _VehicleEditTemplate;
		//*depercated: replaced by '/VehicleManager/html/vehicleEditForm.htm'*/
		public ConfigTemplate VehicleEditTemplate
		{
			get
			{
				if (_VehicleEditTemplate == null) _VehicleEditTemplate = new ConfigTemplate(VehicleEditHtml);
				return _VehicleEditTemplate;
			}
		}
		#endregion

		//! IM-3196
		//deprecated: replaced by html file   "/UnitManager/html/unitEditWindow.html"
		[DataMember]
		[ConfigAssign("02E9D21E-2616-43D9-883C-84126AD2BE35", "<html>Missing 02E9D21E-</html>")]
		public string UnitEditHtml;
		//. IM-3196

		[DataMember]
		[ConfigAssign("16A66A95-7960-4A99-B505-2701B6E0E892", "<html>Missing 16A66A95</html>")]
		public string DriverEditHtml;

		[DataMember]
		[ConfigAssign("7DCC1F70-8B1C-4764-B418-440F2D03172F", "<html>Missing 7DCC1F70</html>")]
		public string FleetEditHtml;

		[DataMember]
		[ConfigAssign("2458F76E-98C2-4F7C-803B-AF9D982EDD9C", "<html>Missing 2458F76E</html>")]
		public string UserEditHtml;

		[DataMember]
		[ConfigAssign("1FEFD817-08EA-432E-B8C2-0FC886964C9B", "<html>Missing 1FEFD817</html>")]
		public string RowExpandHtml;

		//! IM-3196
		//deprecated: replaced by html file   "/JobManager/JobEditWindow.html"
		[DataMember]
		[ConfigAssign("1160C39E-C14E-4f94-A433-372E4722C218", "<html>Missing 1160C39E</html>")]
		public string JobEditHtml;
		//. IM-3196

		//! IM-3196
		//deprecated: replaced by html file   "/JobManager/TaskEditWindow.html" 
		[DataMember]
		[ConfigAssign("44bdc4ae-2572-4877-ace8-93a96dd02db3", "<html>Missing 44bdc4ae</html>")]
		public string JobTaskEditHtml;
		//. IM-3196

		#region Vehicle Settings
		[DataMember]
		[ConfigAssign("e6b97482-df54-4cd2-be3c-d0c6345952fd", "<html>Missing e6b97482</html>")]
		public string VehicleSettingsHtml;		
		[DataMember]
		[ConfigAssign("64627995-E2B1-4a26-9DF3-6341C180F414", "")]
		public string VehicleSettingsData; // key|v1||key2|v2....	1 (plain string, no combination allowed), 11 (concat hierarchy)

		private ConfigTemplate _VehicleSettingsTemplate;
		public string VehicleSettingsInstance
		{
			get
			{
				if (_VehicleSettingsTemplate == null)
				{
					_VehicleSettingsTemplate = new ConfigTemplate(VehicleSettingsHtml);
					VehicleSettingsHtml = null;
				}

				return _VehicleSettingsTemplate.Instantiate(VehicleSettingsData);
			}
		}

		public IDictionary VehicleSettings
		{
			get
			{
				string s = VehicleSettingsData;
				IDictionary _VehicleSettings = s.KeyValueMap(ValueFormat.Strings, true);
				return _VehicleSettings;
			}
		}

		#endregion
		#region Vehicle (Tracking Expand)Setting Config
		[DataMember]
		[ConfigAssign("2876a36e-6d93-4eca-85d5-1dcf261379df", "<html>Missing 2876a36e</html>")]
		public string VehicleSettingConfigHtml;
		[DataMember]
		[ConfigAssign("A5E87D43-503F-4162-BA39-56624406ADED", "")]
		public string VehicleSettingConfigData;
		private ConfigTemplate _VehicleSettingConfigTemplate;
		public string VehicleSettingConfigInstance
		{
			get
			{
				if (_VehicleSettingConfigTemplate == null)
				{
					_VehicleSettingConfigTemplate = new ConfigTemplate(VehicleSettingConfigHtml);
					VehicleSettingConfigHtml = null;
				}

				return _VehicleSettingConfigTemplate.Instantiate(VehicleSettingConfigData);
			}
		}
		public IDictionary VehicleSettingConfig 
		{
			get
			{
				string s = VehicleSettingConfigData ;
				IDictionary _VehicleSettingConfigData = s.KeyValueMap(ValueFormat.Strings, true);
				return _VehicleSettingConfigData;
			}
		}
		#endregion

		//[DataMember]
		//[ConfigAssign("F3400282-9750-40BE-860D-B8BABB31C1AE", "")]
		//public string UnitAttributeDescriptions;

		//TODO MV REPLACE table in Index2.aspx with configuration info.

	}
}
