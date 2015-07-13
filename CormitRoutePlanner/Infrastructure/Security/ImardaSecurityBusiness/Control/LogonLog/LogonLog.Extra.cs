using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;


namespace ImardaSecurityBusiness
{
    partial class ImardaSecurity
    {
        public GetListResponse<LogonLog> GetTopNLogonLogListBySecurityEntityID(IDRequest request)
        {
            var response = new GetListResponse<LogonLog>();
            response.List = new List<LogonLog>();
            try
            {
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<LogonLog>());
                int topN = request.Get("TopN", 50);
								using (IDataReader dr = db.ExecuteDataReader("SPGetTopNLogonLogListBySecurityEntityID", request.ID, topN))
								{
									while (dr.Read()) response.List.Add(GetFromData<LogonLog>(dr));
									return response;
								}
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<LogonLog>>(ex);
            }
        }
    }
}
