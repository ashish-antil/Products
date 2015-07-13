using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	[DataContract]
	[Serializable]
	public class SecurityObjectInfo : BusinessEntity {

		public SecurityObjectInfo() {
		}

		public SecurityObjectInfo(string name, string parent) {
			_name = name;
			_parentId = parent;
		}

		private string _name = null;

		[DataMember]
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		private string _parentId = null;

		[DataMember]
		public string Parent {
			get { return _parentId; }
			set { _parentId = value; }
		}
	}
}
