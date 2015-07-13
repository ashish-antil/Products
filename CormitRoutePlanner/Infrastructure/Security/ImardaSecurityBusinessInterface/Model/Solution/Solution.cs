using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness {
	[DataContract]
	public class Solution : BusinessEntity, IComparable<Solution> {

		#region Instance Variables
		Guid _ID;
		string _SolutionName;
		#endregion

		public Guid ID {
			get { return _ID; }
			set { _ID = value; }
		}

		public string SolutionName {
			get { return _SolutionName; }
			set { _SolutionName = value; }
		}

		public override void AssignData(IDataReader dr) { 
			base.AssignData(dr);

			_ID = (Guid)dr["ID"];
			_SolutionName = GetValue<string>(dr, "SolutionName");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#region IComparable<Solution> Members

		public int CompareTo(Solution other)
		{
			return other == null ? 1 : string.CompareOrdinal(_SolutionName, other.SolutionName);
		}

		#endregion
	}
}
