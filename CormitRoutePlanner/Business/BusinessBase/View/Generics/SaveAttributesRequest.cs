//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ServiceModel;

//namespace FernBusinessBase
//{
//	/// <summary>
//	/// Encapsulates a request to perform some operation, whose function or target will
//	/// differ based on the given ID and parameters.
//	/// </summary>
//	[MessageContract]
//	[Serializable]
//	public class SaveAttributesRequest<T>
//		where T: BusinessEntity
//	{
//		[MessageBodyMember]
//		public Guid ID { get; set; }

//		[MessageBodyMember]
//		public string Data { get; set; }

//		public SaveAttributesRequest()
//		{
//		}

//		public SaveAttributesRequest(Guid id)
//			: this(id, null)
//		{
//		}

//		public SaveAttributesRequest(Guid id, string data)
//		{
//			ID = id;
//			Data = data;
//		}

//		public override string ToString()
//		{
//			return string.Format("SaveAttributesRequest(ID={0}({1}), <<{2}>>)", typeof(T).Name, ID, Data);
//		}


//	}
//}
