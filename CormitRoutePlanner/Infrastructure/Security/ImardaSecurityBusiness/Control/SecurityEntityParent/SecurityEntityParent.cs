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
		public GetListResponse<SecurityEntityParent> GetSecurityEntityParentList()
		{
			try
			{
				return GenericGetEntityList<SecurityEntityParent>(new Guid[] { });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntityParent>>(ex);
			}
		}



		public BusinessMessageResponse SaveSecurityEntityParentList(SaveListRequest<SecurityEntityParent> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				BusinessMessageResponse itemRespnse = new BusinessMessageResponse();
				foreach (SecurityEntityParent it in request.List)
				{
					itemRespnse = GenericSaveEntity<SecurityEntityParent>("SecurityEntityParent", it.RelationId, it.EntityId, it.ParentId, it.Active, it.Deleted);
				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}



		public BusinessMessageResponse DeleteSecurityEntityParent(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<SecurityEntityParent>("EntityParentRelation", new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		/// <summary>
		/// This is a sneaky method called when saving a security entity, to do all the
		/// saves/updates to an entities parent list. This method can throw exceptions.
		/// </summary>
		/// <param name="entityId">The Id of the Entity to proccess the list of parents for.</param>
		/// <param name="parentIds">The list of parents of this entity.</param>
		//internal void SaveSecurityEntityParents(Guid entityId, List<Guid> parentIds) 
		//{

		//	var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
		//	string spName = "SPGetSecurityEntityParentsForEntity";

		//	SaveListRequest<SecurityEntityParent> toSave = new SaveListRequest<SecurityEntityParent>();
		//	toSave.List = new List<SecurityEntityParent>();
		//	BusinessMessageResponse tempResponse = new BusinessMessageResponse();

		//	// run the select
		//	DataSet data = db.ExecuteDataReader(spName, new object[] { entityId });

		//	List<Guid> parentsInDb = new List<Guid>();

		//	foreach (DataRow dr in data.Tables[0].Rows) {
		// 	   parentsInDb.Add((Guid)dr["ParentId"]);
		//	}

		//	while (parentIds.Count > 0) {
		// 	   Guid thisParentId = parentIds[0];

		// 	   if (parentsInDb.Contains(thisParentId)) {
		// 		   // this parent is already setup, just leave it
		// 	   }
		// 	   else {
		// 		   // this parent doesn't exist in the database, create it
		// 		   SecurityEntityParent newParent = new SecurityEntityParent();

		// 		   newParent.RelationId = Guid.NewGuid();
		// 		   newParent.EntityId = entityId;
		// 		   newParent.ParentId = thisParentId;

		// 		   // save this to the list to save
		// 		   toSave.List.Add(newParent);
		// 	   }

		// 	   // done proccessing this one
		// 	   parentIds.RemoveAt(0);
		//	}

		//	// save any that need saving
		//	tempResponse = SaveSecurityEntityParentList(toSave, true);

		//	// if there are any leftover in the db list, that means they have been deleted
		//	while (parentsInDb.Count > 0) {
		// 	   // delete this one
		// 	   IDRequest request = new IDRequest();

		// 	   request.ID = parentsInDb[0];

		// 	   tempResponse = DeleteSecurityEntityParent(request, true);

		// 	   // done proccessing this one
		// 	   parentsInDb.RemoveAt(0);
		//	}
		//}
	}
}