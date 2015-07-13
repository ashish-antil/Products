using System;
using System.Collections.Generic;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;


namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{

		#region DeleteContactMapByContactNPerson
		public BusinessMessageResponse DeleteContactMapByContactNPerson(IDRequest request)
		{
			try
			{
				const string spName = "SPDeleteContactMapByContactNPerson";
				var response = new BusinessMessageResponse();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ContactMap>());
				var contactId = request["ContactId"];
				var contactPersonId = request["ContactPersonId"];
				var args = new object[] { new Guid(contactId), new Guid(contactPersonId), request.CompanyID };
				if (db.ExecuteNonQuery(spName, args) > 0)
				{
					response.Status = true;
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ContactMap>>(ex);
			}
		}
		#endregion
	}
}
