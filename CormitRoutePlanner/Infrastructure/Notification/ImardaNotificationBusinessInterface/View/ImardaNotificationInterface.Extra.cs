using FernBusinessBase;
using System.ServiceModel;

namespace ImardaNotificationBusiness
{
	partial interface IImardaNotification 
	{
		[OperationContract]
		BusinessMessageResponse SendEmail(GenericRequest req);
		[OperationContract]
		BusinessMessageResponse SendSMS(GenericRequest req);
		[OperationContract]
		BusinessMessageResponse SendSingle(GenericRequest req);
		[OperationContract]
		SimpleResponse<string> HttpPost(GenericRequest req);

		//& IM-3927
		[OperationContract]
		BusinessMessageResponse SendsFTP(GenericRequest req);
		//. IM-3927
	}
}
