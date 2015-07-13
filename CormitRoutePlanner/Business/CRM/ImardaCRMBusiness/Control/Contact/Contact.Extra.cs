using System;
using System.Collections.Generic;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		#region SearchContactList
		public GetListResponse<Contact> SearchContactList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<Contact>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Contact>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				int topn = request.Get<int>("TopN", int.MaxValue);
				int contactType = request.Get<int>("ContactType", 0);
				string searchContent = request.GetString("SearchContent") ?? "";
				using (IDataReader dr = db.ExecuteDataReader("SPSearchContactList", includeInactive, request.CompanyID, topn, contactType, searchContent))
				{
					response.List = new List<Contact>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<Contact>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		#endregion

		#region GetContactListByParentId
		public GetListResponse<Contact> GetContactListByParentId(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<Contact>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Contact>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				int topn = request.Get<int>("TopN", int.MaxValue);
				int contactType = request.Get<int>("ContactType", 0);
				Guid parentId = request.Get("ParentId", Guid.Empty);
				using (IDataReader dr = db.ExecuteDataReader("SPGetContactListByParentId", includeInactive, request.CompanyID, topn, contactType, parentId))
				{
					response.List = new List<Contact>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<Contact>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		#endregion
		#region DeleteContactRelatedPersons
		public BusinessMessageResponse DeleteContactRelatedPersons(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<Contact>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Contact>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);

				db.ExecuteNonQuery("SPDeleteContactRelatedPersons", includeInactive, request.CompanyID, request.ID);
				response.Status = true;
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		#endregion



        #region GetContactByName


        public GetItemResponse<Contact> GetContactByName(IDRequest request)
        {

            try
            {
                var response = new GetItemResponse<Contact>();
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Contact>());
                string custName = request.GetString("custName");

                using (IDataReader dr = db.ExecuteDataReader("SPGetContactByName", request.ID, custName))
                {
                    response = new GetItemResponse<Contact>();

                    if (dr.Read())
                    {
                        response.Item = (GetFromData<Contact>(dr));
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Contact>>(ex);
            }






        }



        #endregion
    }
}
