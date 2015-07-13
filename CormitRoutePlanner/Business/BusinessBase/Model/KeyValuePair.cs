using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Runtime.Serialization;
using System.Collections.Generic;

/// <summary>
/// A Key / value pair that can be transported over the web service.
/// </summary>
[DataContract]
public class KeyValuePair<Tkey, Tvalue> {
	private Tkey _Key;
	private Tvalue _Value;

	[DataMember]
	public Tvalue Value {
		get { return _Value; }
		set { _Value = value; }
	}

	[DataMember]
	public Tkey Key {
		get { return _Key; }
		set { _Key = value; }
	}
}
