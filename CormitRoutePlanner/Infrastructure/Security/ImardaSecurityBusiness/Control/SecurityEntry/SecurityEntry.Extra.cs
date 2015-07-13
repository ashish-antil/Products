using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{

		public BusinessMessageResponse DeleteEntriesByEntityID(IDRequest request)
		{
			try
			{
				Guid securityEntityID = request.ID;
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntry>());
				db.ExecuteNonQuery("SPDeleteEntriesByEntityID", securityEntityID);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


		public GetListResponse<SecurityEntry> GetEntriesByEntityID(IDRequest request)
		{
			try
			{
				return base.GenericGetRelated<SecurityEntry>("EntityID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntry>>(ex);
			}
		}

		
		/// <summary>
		/// Delete security entries not by SecurityEntry.ID but by (.EntityID, .SecurityID)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public BusinessMessageResponse DeleteSecurityEntryList(SaveListRequest<SecurityEntryKey> request)
		{
			try
			{
				Guid userID;
				if (!request.Get<Guid>("UserID", out userID)) userID = Guid.Empty;

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntry>());
				foreach (SecurityEntryKey key in request.List)
				{
					db.ExecuteNonQuery("SPDeleteSecurityEntryByFKey", key.EntityID, key.SecurityObjectID, userID);
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


	}
}
