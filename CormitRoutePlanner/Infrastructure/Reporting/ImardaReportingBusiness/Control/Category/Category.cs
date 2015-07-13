using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Errors;

namespace ImardaReportBusiness
{
	partial class ImardaReport
	{
		public GetListResponse<Category> GetCategoryList()
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Category>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetCategoryList", new object[0]))
				{
					var list = new List<Category>();
					while (dr.Read()) list.Add(GetFromData<Category>(dr));
					return SuccessList<Category>(list);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Category>>(ex);
			}

		}
	}
}
