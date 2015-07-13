using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification 
	{

		public BusinessMessageResponse SendPlan(GenericRequest req)
		{
			try
			{
				var response = new BusinessMessageResponse();
				//separate delivery method from request
				//call SendEmail(req1);
				//call SendSMS(req2);
				//call other methods;
				return response;// always return successful response
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		private void SaveToPending()
		{
			throw new NotImplementedException();
		}

		BusinessMessageResponse SendEmail(GenericRequest req)
		{
			try
			{
				var response = new BusinessMessageResponse();
				//for each recipient in req
				//build a new Email object
				//SaveToPending();
				//SendSingleEmail(Email e);

				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		BusinessMessageResponse SendSMS(GenericRequest req) 
		{
			try
			{
				var response = new BusinessMessageResponse();

				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
