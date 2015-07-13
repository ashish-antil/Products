using System.Runtime.Serialization;
using FernBusinessBase;
using System;
using Imarda.Lib;

// ReSharper disable once CheckNamespace
namespace ImardaConfigurationBusiness 
{
	[DataContract]
	[KnownType(typeof(Measurement))]
	public class ConfigValue : BaseEntity
	{
		[DataMember]
		public int Type { get; set; }
		
		[DataMember]
		public object Value  { get; set; }

		[DataMember]
		public Guid UID { get; set; }

		public override string ToString()
		{
			return string.Format("ConfigValue({0},{1},{2})", UID, Type, Value);
		}
	}

    [DataContract]
    public sealed class ConfigValueAndDescr
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Descr { get; set; }
    }
}
