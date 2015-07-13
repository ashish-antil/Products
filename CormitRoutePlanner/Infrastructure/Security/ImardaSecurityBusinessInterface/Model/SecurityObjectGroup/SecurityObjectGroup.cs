using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	[Serializable]
	public class SecurityObjectGroup : FullBusinessEntity
	{

		#region Instance Variables
		private string _Name;
		private string _Description;
		private int _Visibility;
		#endregion

		public const int NameMaxLen = 50;
		[ValidLength(-1, NameMaxLen)]
		[DataMember]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		[DataMember]
		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		[DataMember]
		public int Visibility
		{
			get { return _Visibility; }
			set { _Visibility = value; }
		}

		public SecurityObjectGroup()
		{
		}

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);

			_Name = GetValue<string>(dr, "Name");
			_Description = GetValue<string>(dr, "Description");
			_Visibility = GetValue<int>(dr, "Visibility");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}
	}
}
