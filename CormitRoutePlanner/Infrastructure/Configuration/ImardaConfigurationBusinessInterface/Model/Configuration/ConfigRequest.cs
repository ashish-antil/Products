using System;
using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;


namespace ImardaConfigurationBusiness
{
	[MessageContract]
	public class ConfigRequest : GenericRequest
	{
		private enum ValueTypeEnum
		{
			Unspecified = 0,
			Measurement = 1,
		}

		public ConfigRequest()
		{
		}

		public ConfigRequest(Guid itemID, object appParameter, params Guid[] levels)
		{
			ID = itemID;
			Parameters = new object[1 + levels.Length];
			AppParameter = appParameter;
			for (int i = 0; i < levels.Length; i++) Parameters[1 + i] = levels[i];
		}

		public object AppParameter
		{
			get { return DeserializeAppParameter(); }
			set { Parameters[0] = SerializeAppParameter(value); }
		}

		[MessageBodyMember]
		public string Notes { get; set; }

		[MessageBodyMember]
		public byte ValueType { get; set; }

		[MessageBodyMember]
		public bool Combine { get; set; }

		[MessageBodyMember]
		public bool IgnoreCache { get; set; }

		[MessageBodyMember]
		private ValueTypeEnum _AppParameterType;

		public Guid[] GetLevels()
		{
			int n = Parameters.Length - 1;
			Guid[] levels = new Guid[n];
			for (int i = 0; i < n; i++) levels[i] = (Guid)Parameters[1 + i];
			return levels;
		}

		public int Depth
		{
			get { return Parameters.Length - 1; }
		}

		private object SerializeAppParameter(object obj)
		{
			if (obj is IMeasurement)
			{
				_AppParameterType = ValueTypeEnum.Measurement;
				return Measurement.PString((IMeasurement)obj);
			}
			else return obj;
		}

		private object DeserializeAppParameter()
		{
			switch (_AppParameterType)
			{
				case ValueTypeEnum.Measurement:
					return Measurement.Parse((string)Parameters[0]);
				default:
					return Parameters[0];
			}
		}

	}
}
