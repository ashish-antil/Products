using System.Collections;
using System.Linq;
using Imarda.Lib.MVVM.Common;
using Imarda.Logging;

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using FernBusinessBase.Errors;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    /// <summary>
    ///     Provides methods that any business service contract class can use to help things along.
    /// </summary>
    public abstract class BusinessBase : Disposable, IServerFacadeBase
    {
        public static string BusinessServiceName;

        private readonly EntityCacheManager _EntityCacheManager = new EntityCacheManager(BusinessServiceName);     //& IM-5178
        private readonly AttributeCacheManager _AttributeCacheManager = new AttributeCacheManager(BusinessServiceName);     //& IM-5178
		#region IServerFacadeBase Members

		/// <summary>
		/// Override in subclass.
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		public virtual SimpleResponse<string> GetAttributes(IDRequest req)
		{
			try
			{
				throw new NotImplementedException("GetAttributes not implemented service on " + GetType().FullName);
			}
			catch (Exception ex)
			{
				return new SimpleResponse<string>(ErrorHandler.Handle(ex));
			}
		}

        public virtual SimpleResponse<string> GetVehicleAttributesForEntityId(IDRequest req)
        {
            try
            {
                throw new NotImplementedException("GetVehicleAttributesForEntityId not implemented service on " + GetType().FullName);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<string>(ErrorHandler.Handle(ex));
            }
        }

        public SimpleResponse<string[]> GetVehicleAttributesForEntities(IDListRequest req)
        {
            throw new NotImplementedException();
        }

        public virtual SimpleResponse<string> GetDriverAttributesForEntityId(IDRequest req)
        {
            try
            {
                throw new NotImplementedException("GetDriverAttributesForEntityId not implemented service on " + GetType().FullName);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<string>(ErrorHandler.Handle(ex));
            }
        }

        public SimpleResponse<string[]> GetDriverAttributesForEntities(IDListRequest req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// General Ping method, called to check if the service is running fine, and to get metrics  
		/// returned in ahd key/value list.
		/// </summary>
		/// <param name="query">contains data that indicate what key/value pairs to retrieve</param>
		/// <returns></returns>
		public KeyValueListResponse Ping(IDRequest query)
		{
			try
			{
				var resp = PingSpecific(query);
				resp.AddParameters("Service", GetType().Name, "Machine", Environment.MachineName); // add here more general info if required
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<KeyValueListResponse>(ex);
			}
		}

		/// <summary>
		/// Override this method in all subclasses with an implementation in each Service.
		/// By default this method returns an empty list.
		/// It is called by Ping() which adds some general information to it.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		protected virtual KeyValueListResponse PingSpecific(IDRequest query)
		{
			return new KeyValueListResponse();
		}

		#endregion

		#region GetFromData<T>

		/// <summary>
		/// Gets a new business entity of the given type from the data row.
		/// </summary>
		/// <typeparam name="T">The Type of BusinessEntity to return</typeparam>
		/// <param name="dr">The DataRow from which the entity will be created</param>
		/// <returns>A new BusinessEntity of the given Type, constructed from the DataRow.</returns>
		protected T GetFromData<T>(IDataReader dr)
			where T : BusinessEntity, new()
		{
			var entity = new T();
			entity.AssignData(dr);
			return entity;
		}

		protected T GetFromData<T>(IDataReader dr, bool includeAttributes)
			where T : FullBusinessEntity, new()
		{
			var entity = new T();
			entity.AssignData(dr);
			if (includeAttributes)
			{
                //int i = dr.GetOrdinal("Attributes");                                      // old attribute code
                //if (i >= 0) entity.Attributes = new EntityAttributes(dr[i] as string);    // old attribute code
			    PopulateEntityAttributes(entity);   // IM-3747
			}
			return entity;
		}

        /// <summary>
        /// Temp method to populate an entity with the old format of attribute data, retrieved from new attribute cache // IM-3747
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected void PopulateEntityAttributes<T>(T entity)   //IM-3747
            where T: FullBusinessEntity
        {
            if (entity == null) return;

            string entityName = typeof(T).Name;

            switch (entityName)
            {
                case "LiteTrackable":
                case "Trackable":
                case "LiteUnit":
                    entityName = "Unit";    // attribute cache only has Units
                    break;
            }

            // Get all attribute values for this entity (e.g. Unit) for a specific Unit (ID)
            List<AttributeValue> attributeValueList = _AttributeCacheManager.GetEntityAttributeListFromAttributeValueCache(entityName, entity.ID);
            if (attributeValueList == null) attributeValueList = new List<AttributeValue>();    // if this is a new unit, then no values will be available

            // Now get all attribute definitions for the company's entity (e.g. Unit) and if any are missing from the attribute values (because of no value yet)
            // then add them. This is to ensure we have a full compliment of all possible attributes for the entity (e.g. Unit), whether or not there is a value yet.
            // NOTE - this is not ideal and is a hack to ensure sb strings contain all attributes for the pipe delimited way of dealing with attributes
            // (when everything is moved over to use the AttributeDefintion and AttributeValue classes, then this whole method becomes redundant).
            var attributeDefinitionList = _AttributeCacheManager.GetEntityListFromAttributeDefinitionCache(entityName,entity.CompanyID);
            if (attributeDefinitionList == null) return;
            foreach (var ad in attributeDefinitionList)
            {
                var attributeValue = attributeValueList.FirstOrDefault(s => (s.VarName.ToLower() == ad.VarName.ToLower()));

                if (attributeValue == null) // there is no attribute value for this attribute definition, so add one
                {
                    attributeValue = new AttributeValue();
                    attributeValue.ID = Guid.NewGuid();
                    attributeValue.DateModified = DateTime.UtcNow;
                    attributeValue.AttributeID = ad.ID;
                    attributeValue.EntityID = entity.ID;
                    attributeValue.Value = DetermineDefaultValueForAttribute(ad.VarType);
                    attributeValue.VarName = ad.VarName;
                    attributeValue.FriendlyName = ad.FriendlyName;
                    attributeValue.Description = ad.Description;
                    attributeValue.GroupID = ad.GroupID;
                    attributeValue.VarType = ad.VarType;
                    attributeValue.Format = ad.Format;
                    attributeValue.CaptureHistory = ad.CaptureHistory;
                    attributeValue.Viewable = ad.Viewable;
                    attributeValue.EntityTypeName = ad.EntityTypeName;
                    attributeValueList.Add(attributeValue);
                    // we won't save this to cache or disk, as caller may never populate it with a value
                }
            }

            if (attributeValueList != null && attributeValueList.Count > 0)
            {
                // populate in new format, as we can use this more and more going forward, and it's needed when prepping to call action engine
                entity.AttributeValues = attributeValueList;

                // take new attribute format and change into old format of a pipe delimited string and add to response object
                string pipedString = ConvertAttributeValueListToPipedString(attributeValueList);
                entity.Attributes = new EntityAttributes(pipedString);
            }
        }

        private string DetermineDefaultValueForAttribute(string varType)
        {
            if (varType.StartsWith("!")) return "0";
            switch (varType)
            {
                case "*": return Guid.Empty.ToString();
                case "$": return "";
                case "@": return "\u00B6d";     //DateTime.MinValue.ToString();
                default: return "0";
            }
        }

		//protected T GetFromDataReader<T>(IDataReader dr)
		//  where T: BusinessEntity, new()
		//{
		//  if (!dr.Read()) return default(T);
		//  var entity = new T();
		//  entity.AssignData(dr);
		//  return entity;
		//}

		#endregion

		#region GenericGetEntityList<T>

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned. Must inherit from BusinessEntity</typeparam>
		/// <param name="entityName">The name of the entity to build the SP name from.</param>
		/// <param name="ids"></param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntityList<T>(string entityName, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityList<T>(entityName, false, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned. Must inherit from BusinessEntity</typeparam>
		/// <param name="entityName">The name of the entity to build the SP name from.</param>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="ids"></param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntityList<T>(string entityName, bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

				var args = new object[ids.Length];
				for (int i = 0; i < ids.Length; i++)
				{
					args[i] = ids[i];
				}

				spName = "SPGet" + entityName + "List";
				using (var dr = db.ExecuteDataReader(spName, args))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				if (rethrow) throw;
				return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, ids);
			}
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="spName"></param>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="ids"></param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntities<T>(string spName, bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

				using (IDataReader dr = db.ExecuteDataReader(spName, ids))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				if (rethrow) throw;
				return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, ids);
			}
		}

	    protected GetListResponse<T> GenericGetEntities<T>(string spName, List<object> parameters)
		    where T : BusinessEntity, new()
	    {
		    return GenericGetEntities<T>(spName,false,  parameters);
	    }

		/// <summary>
		/// Gets a list of entities of the given Type that matches the given parameters.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="spName"></param>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="parameters"></param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntities<T>(string spName, bool rethrow, List<object> parameters)
			where T : BusinessEntity, new()
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

				var args = new object[parameters.Count];
				for (int i = 0; i < parameters.Count; i++)
				{
					if (parameters[i] is DateTime)
					{
						parameters[i] = ReadyDateForStorage((DateTime)parameters[i]);
					}
					args[i] = parameters[i];
				}
				spName = "SP" + spName;
				using (IDataReader dr = db.ExecuteDataReader(spName, args))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				if (rethrow)
					throw;
				else
				{
					return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, parameters);
				}
			}
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// The SP name is built according to "SPGet" + typeof(T).Name + "List".
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned. Must inherit from BusinessEntity</typeparam>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntityList<T>(params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityList<T>(false, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids.
		/// Will work fine, assuming the database Stored Procedures are written according to convention:
		/// The SP name is built according to "SPGet" + typeof(T).Name + "List".
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned. Must inherit from BusinessEntity</typeparam>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="ids"></param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurrs calling the stored procedure.</exception>
		protected GetListResponse<T> GenericGetEntityList<T>(bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityList<T>(typeof(T).Name, ids);
		}

		protected GetListResponse<T> GenericGetEntityList<T>(IDRequest companyIDRequest)
			where T : FullBusinessEntity, new()
		{
			string entityName = typeof(T).Name;
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				bool includeAttributes = companyIDRequest.HasSome(RetrievalOptions.Attributes);
				bool includeInactive = companyIDRequest.HasSome(RetrievalOptions.IncludeInactive);

                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
                string prefix = includeAttributes ? "SPGetX" : "SPGet";
                spName = prefix + entityName + "List";
				//spName = "SPGet" + entityName + "List";     //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
				using (IDataReader dr = db.ExecuteDataReader(spName, includeInactive, companyIDRequest.ID))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr, includeAttributes));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, companyIDRequest);
			}
		}

        public GetListResponse<T> GenericGetCachedEntityList<T>(IDRequest companyIDRequest)
			where T : FullBusinessEntity, new()
		{
            bool includeAttributes = companyIDRequest.HasSome(RetrievalOptions.Attributes);
            bool includeInactive = companyIDRequest.HasSome(RetrievalOptions.IncludeInactive);
			string entityName = typeof(T).Name;
            var entityList =  _EntityCacheManager.GetEntityItemListFromEntityCache(entityName, companyIDRequest.ID, includeInactive);
            if (entityList != null && entityList.Count > 0)
            {
                var response = new GetListResponse<T> { List = new List<T>() };
                response.List.AddRange(entityList.Cast<T>().ToList());
                return response;
            }
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
				string prefix = includeAttributes ? "SPGetX" : "SPGet";
				spName = prefix + entityName + "List";
                //spName = "SPGet" + entityName + "List";     //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
			    using (IDataReader dr = db.ExecuteDataReader(spName))   //, includeInactive, companyIDRequest.ID))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr, includeAttributes));
					}
                    if (response.List.Count > 0)
                        _EntityCacheManager.AddEntityItemListToEntityCache(entityName, response.List.Cast<FullBusinessEntity>().ToList());
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, companyIDRequest);
			}
		}

        public bool LoadCachedAttributeListForBusinessService<T>(IDRequest request)
        {
            string businessServiceName = request.GetString("BusinessServiceName") ?? "";
            LoadCachedAttributeDefinitionListForBusinessService(businessServiceName);
            LoadCachedAttributeValueListForBusinessService(businessServiceName);
            return true;
        }

        /// <summary>
        /// Only called once at start-up of business service
        /// </summary>
        /// <param name="businessServiceName"></param>
        /// <returns></returns>
        public bool LoadCachedAttributeDefinitionListForBusinessService(string businessServiceName)
        {
            try
            {
                var db = ImardaDatabase.CreateDatabase("ImardaAttributingBusinessInterface");
                string spName = "SPGetAttributeDefinitionListByBusinessService";
                using (IDataReader dr = db.ExecuteDataReader(spName, businessServiceName))
                {
                    var attributeDefinitionList = new List<AttributeDefinition>();
                    string currentEntityType = "";
                    while (dr.Read())
                    {
                        var av = GetFromData<AttributeDefinition>(dr);
                        if (av != null)
                        {
                            if (currentEntityType == "" || av.EntityTypeName == currentEntityType)
                            {
                                currentEntityType = av.EntityTypeName;
                                attributeDefinitionList.Add(av);
                            }
                            else
                            {
                                if (attributeDefinitionList.Count > 0)
                                    _AttributeCacheManager.AddEntityItemListToAttributeDefinitionCache(currentEntityType, attributeDefinitionList);
                                attributeDefinitionList.Clear();
                                currentEntityType = av.EntityTypeName;
                                attributeDefinitionList.Add(av);
                            }
                        }
                    }
                    if (attributeDefinitionList.Count > 0)
                        _AttributeCacheManager.AddEntityItemListToAttributeDefinitionCache(currentEntityType, attributeDefinitionList);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DebugLog.Write(ex);
                return false;
            }
        }

        /// <summary>
        /// Only called once at start-up of a business service
        /// </summary>
        /// <param name="businessServiceName"></param>
        /// <returns></returns>
        public bool LoadCachedAttributeValueListForBusinessService(string businessServiceName)
        {
            try
            {
                var db = ImardaDatabase.CreateDatabase("ImardaAttributingBusinessInterface");  //("Imarda360.Attributing");
                string spName = "SPGetAttributeValueListByBusinessService";
                // NOTE - expects list sorted by entity name, by entity id - otherwise below logic won't work
                using (IDataReader dr = db.ExecuteDataReader(spName, businessServiceName))
                {
                    var attributeValueList = new List<AttributeValue>();
                    string currentEntityType = "";
                    Guid currentEntityID = Guid.Empty;
                    while (dr.Read())
                    {
                        var av = GetFromData<AttributeValue>(dr);
                        if (av != null)
                        {
                            if ((currentEntityType == "" || av.EntityTypeName == currentEntityType) && (currentEntityID == Guid.Empty || av.EntityID == currentEntityID))
                            {
                                currentEntityType = av.EntityTypeName;
                                currentEntityID = av.EntityID;
                                attributeValueList.Add(av);
                            }
                            else
                            {
                                if (attributeValueList.Count > 0)
                                    _AttributeCacheManager.AddEntityItemListToAttributeValueCache(currentEntityType, currentEntityID, attributeValueList);
                                attributeValueList = new List<AttributeValue>();
                                currentEntityType = av.EntityTypeName;
                                currentEntityID = av.EntityID;
                                attributeValueList.Add(av);
                            }
                        }
                    }
                    if (attributeValueList.Count > 0)
                        _AttributeCacheManager.AddEntityItemListToAttributeValueCache(currentEntityType, currentEntityID, attributeValueList);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DebugLog.Write(ex);
                return false;
            }
        }

		/// <summary>
		/// Calls an SP named SPGetEntityListByParamID - supports SPGetX
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="idRequest">ID to select</param>
		/// <param name="paramId">Name of the column containing ID to select</param>
		/// <returns></returns>
		protected GetListResponse<T> GenericGetEntityList<T>(IDRequest idRequest, string paramId)
	where T : FullBusinessEntity, new()
		{
			var entityName = typeof(T).Name;
			var spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				var includeAttributes = idRequest.HasSome(RetrievalOptions.Attributes);
				var includeInactive = idRequest.HasSome(RetrievalOptions.IncludeInactive);

                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
				var prefix = includeAttributes ? "SPGetX" : "SPGet";
				spName = prefix + entityName + "ListBy" + paramId;
                //spName = "SPGet" + entityName + "ListBy" + paramId; //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
				using (var dr = db.ExecuteDataReader(spName, includeInactive, idRequest.ID))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr, includeAttributes));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, idRequest);
			}
		}

		#endregion

		#region Relationships

		private static readonly Regex _DatabaseNameRegex = new Regex(@"(?<=Imarda).*(?=Business)",
																																 RegexOptions.Compiled | RegexOptions.CultureInvariant);

		/// <summary>
		/// Get entities that share the same given foreign key.
		/// </summary>
		/// <typeparam name="T">entity type to retrieve</typeparam>
		/// <param name="fkColumn">name of database column</param>
		/// <param name="id">foreign key value</param>
		/// <param name="includeDeleted">true to include deleted records, false to filter out the deleted records</param>
		/// <returns></returns>
		protected GetListResponse<T> GenericGetRelated<T>(string fkColumn, Guid id, bool includeDeleted)
			where T : FullBusinessEntity, new()
		{
		    string query = null;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				Type type = typeof(T);
				string tableName = type.Name;
				string ns = type.Namespace; //e.g. ImardaTrackingBusiness, ImardaCRMBusiness
				string database = _DatabaseNameRegex.Match(ns).Value;
				string filter = includeDeleted ? string.Empty : " AND [Deleted] = 0 ";
				// Although table variables exist in T-SQL, there is no column variable, therefore
				// we have to construct the SQL query here.
				query = string.Format(
					"USE [Imarda360.{0}];" +
					" SELECT * FROM [{1}]" +
					" WHERE [{2}] = '{3}' {4}" +
					" ORDER BY [DateCreated]",
					database, tableName, fkColumn, id, filter);

				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
                if (query != null)
                {
                    ErrorHandler.ErrorFormat("sql:{0}|ex:{1}", query, ex);
                }
                return ErrorHandler.Handle<GetListResponse<T>>(ex, typeof(T), fkColumn, id, includeDeleted);
			}
		}

		/// <summary>
		/// Given three tables A, A_B and B where A_B establishes a many-to-many relationship between A and B,
		/// then this method will find all B if an A record is given, or it finds all A if a B record is given.
		/// </summary>
		/// <typeparam name="T">target table (B)</typeparam>
		/// <param name="originTableName">the table (A) of the record whose primary key (id) is given.</param>
		/// <param name="id">primary key in (A) and foreign key in (B)</param>
		/// <param name="direction">Normal when namewise (A_B: A->B) and Inverse if counternamewise (B->A)</param>
		/// <param name="includeDeleted">true to include deleted records, false to filter them out</param>
		/// <returns>a list of entities of the given type T (B)</returns>
		protected GetListResponse<T> GenericGetRelated<T>(string originTableName, Guid id, Relationship direction,
																											bool includeDeleted)
			where T : FullBusinessEntity, new()
		{
            string query = null;
            try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				Type type = typeof(T);
				string targetTableName = type.Name;
				string joinTableName = direction == Relationship.Normal
																? originTableName + '_' + targetTableName
																: targetTableName + '_' + originTableName;
				string ns = type.Namespace; //e.g. ImardaTrackingBusiness, ImardaCRMBusiness
				string database = _DatabaseNameRegex.Match(ns).Value;
				string filter = includeDeleted ? string.Empty : " AND x.[Deleted] = 0 AND r.[Deleted] = 0 ";
				// Although table variables exist in T-SQL, there is no column variable, therefore
				// we have to construct the SQL query here.
				query = string.Format(
					"USE [Imarda360.{0}];" +
					" SELECT * FROM [{1}] AS r" +
					" INNER JOIN [{2}] AS x" +
					" ON r.ID = x.[{1}ID] " +
					" WHERE x.[{3}ID] = '{4}' {5}" +
					" ORDER BY r.[DateCreated]",
					database, targetTableName, joinTableName, originTableName, id, filter
					);
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
                if (query != null)
                {
                    ErrorHandler.ErrorFormat("sql:{0}|ex:{1}", query, ex);
                }
                return ErrorHandler.Handle<GetListResponse<T>>(ex, typeof(T), originTableName, id, direction, includeDeleted);
			}
		}

		/// <summary>
		/// Indicates join table traversal direction. See example.
		/// In general if the join table is names "A_B" then going from A->B is Normal and B->A is inverse,
		/// so the RelationShip direction is fully based on the join table name.
		/// </summary>
		/// <example>
		/// If join table is named Item_Category then finding Categories from a given Item is 'Normal'
		/// whereas finding the Items in a Category is 'Inverse'.
		/// </example>
		protected enum Relationship
		{
			Normal,
			Inverse
		}

		#endregion Relationships

		#region GenericGetExtent<T>

		protected GetListResponse<T> GenericGetExtent<T>(
			Guid? companyID,
			DateTime? createdAfter,
			DateTime? createdBefore,
			DateTime? modifiedAfter,
			DateTime? modifiedBefore,
			bool? deleted,
			bool? active,
			bool? isTemplate,
			string path,
			int? limit,
			int? offset,
			List<SortColumn> sortColumns,
			Guid? ownerID,
			int? ownerType
			)
			where T : FullBusinessEntity, new()
		{
		    StringBuilder sb = null;
			try
			{
				bool sql2012 = false;  //not used everywhere yet

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				Type type = typeof(T);
				string tableName = type.Name;
				string ns = type.Namespace; //e.g. ImardaTrackingBusiness, ImardaCRMBusiness
				string database = _DatabaseNameRegex.Match(ns).Value;
				string[] filters =
					{
						companyID.HasValue && companyID.Value != Guid.Empty ?string.Format(" [CompanyID] = '{0}' ", companyID.Value.ToString()) : null,
						createdAfter.HasValue ? string.Format(" [DateCreated] >= '{0}' ", createdAfter.Value.ToString("s")) : null,
						createdBefore.HasValue ? string.Format(" [DateCreated] <= '{0}' ", createdBefore.Value.ToString("s")) : null,
						modifiedAfter.HasValue ? string.Format(" [DateModified] >= '{0}' ", modifiedAfter.Value.ToString("s")) : null,
						modifiedBefore.HasValue ? string.Format(" [DateModified] <= '{0}' ", modifiedBefore.Value.ToString("s")) : null,
						deleted.HasValue ? " [Deleted] = " + (deleted.Value ? '1' : '0') : null,
						active.HasValue ? " [Active] = " + (active.Value ? '1' : '0') : null,
						isTemplate.HasValue ? " [IsTemplate] = " + (isTemplate.Value ? '1' : '0') : null,
						!string.IsNullOrEmpty(path) ? " ([Path] like '" + path +  "%' OR [Path] IS NULL OR [Path] = '')" : null,
						ownerID.HasValue && ownerID.Value != Guid.Empty ?string.Format(" [OwnerID] = '{0}' ", ownerID.Value.ToString()) : null,
						ownerType.HasValue ? " [OwnerType] = " + ownerType.Value.ToString() : null,
					};

				//build sort clause
				var sortClause = "";
				var con = "";
				if (sortColumns != null)
				{
					foreach (SortColumn sortColumn in sortColumns)
					{
						var part = sortColumn.FieldName;
						sortClause += con + part;
						if (sortColumn.SortDescending) sortClause += " DESC";
						con = ", ";
					}
				}
				if (string.IsNullOrEmpty(sortClause))
					sortClause = "ID"; //we need some column for the OVER part

				//build filter clause
				var s = " WHERE ";
				var filterClause = new StringBuilder();
				foreach (var filter in filters)
				{
					if (filter != null)
					{
						filterClause.Append(s).Append(filter);
						s = " AND ";
					}
				}

				sb = new StringBuilder(string.Format("USE [Imarda360.{0}]; ", database));
				if (limit.HasValue && limit.Value > 0)
				{
					var from = offset.Value;
					if (from < 1) from = 1;
					var to = Int32.MaxValue;
					if (limit.Value > 0)
						to = from + limit.Value - 1;

					if (sql2012)
					{
						//only usable for SQLS2012
						//if (offset.HasValue && offset.Value > 0)
						//	sb.Append(string.Format(" OFFSET {0} ROWS", offset.Value));
						//sb.Append(string.Format(" FETCH NEXT {0} ROWS ONLY", limit.Value));
					}
					else
					{
						sb.Append("SELECT * FROM (");
						sb.Append(string.Format("SELECT t.*, ROW_NUMBER() OVER (ORDER BY {1}) AS RowNum FROM [{0}] t ", tableName, sortClause));
						sb.Append(filterClause.ToString());
						sb.Append(") AS SubTable ");
						sb.Append(string.Format(" WHERE SubTable.RowNum BETWEEN {0} AND {1}", from, to));
					}
				}
				else
				{
					sb.Append(string.Format("SELECT * FROM [{0}] ", tableName));
					sb.Append(filterClause.ToString());
				}

				string query = sb.ToString();
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
                if (sb != null)
                {
                    ErrorHandler.ErrorFormat("sql:{0}|ex:{1}", sb, ex);
                }
                return ErrorHandler.Handle<GetListResponse<T>>(ex, typeof(T));
			}
		}

		#endregion

		#region GenericGetExtentWithCustomSelect<T>

		protected GetListResponse<T> GenericGetExtentWithCustomSelect<T>(
			Guid? companyID,
			DateTime? createdAfter,
			DateTime? createdBefore,
			DateTime? modifiedAfter,
			DateTime? modifiedBefore,
			bool? deleted,
			bool? active,
			bool? isTemplate,
			string path,
			int? limit,
			int? offset,
			List<SortColumn> sortColumns,
			Guid? ownerID,
			int? ownerType,
			string innerSelect,
			List<Condition> conditions,
			ConditionLogicalOperator logicalOperator
			)
			where T : FullBusinessEntity, new()
		{
		    StringBuilder sb = null;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				Type type = typeof(T);
				string tableName = type.Name;
				string ns = type.Namespace; //e.g. ImardaTrackingBusiness, ImardaCRMBusiness
				string database = _DatabaseNameRegex.Match(ns).Value;

				//build where clause
				var whereClause = "";
				var con = "";
				//base filtering
				string[] filters =
					{
						//companyID.HasValue && companyID.Value != Guid.Empty ?string.Format(" [CompanyID] = '{0}' ", companyID.Value.ToString()) : null,
						createdAfter.HasValue ? string.Format(" [DateCreated] >= '{0}' ", createdAfter.Value.ToString("s")) : null,
						createdBefore.HasValue ? string.Format(" [DateCreated] <= '{0}' ", createdBefore.Value.ToString("s")) : null,
						modifiedAfter.HasValue ? string.Format(" [DateModified] >= '{0}' ", modifiedAfter.Value.ToString("s")) : null,
						modifiedBefore.HasValue ? string.Format(" [DateModified] <= '{0}' ", modifiedBefore.Value.ToString("s")) : null,
						deleted.HasValue ? " [Deleted] = " + (deleted.Value ? '1' : '0') : null,
						active.HasValue ? " [Active] = " + (active.Value ? '1' : '0') : null,
					};
				//these filters are 'AND'd together
				var filterClause = new StringBuilder();
				con = "";
				foreach (var filter in filters)
				{
					if (filter != null)
					{
						filterClause.Append(con).Append(filter);
						con = " AND ";
					}
				}

				//template we need handle differently because we treat null as false 
				if (isTemplate.HasValue)
				{
					if (isTemplate.Value == true)
					{
						filterClause.Append(con).Append(" [IsTemplate] = 1 ");
					}
					else
					{
						filterClause.Append(con).Append(" ([IsTemplate] = 0 OR [IsTemplate] IS NULL) ");
					}
				}
									 

				//Add any further conditions 
				var conditionClause = new StringBuilder();
				con = "";

			    if (conditions != null)
			    {
			        foreach (var condition in conditions)
			        {
			            var part = condition.GetSQL();
			            conditionClause.Append(con).Append(part);
			            con = logicalOperator == ConditionLogicalOperator.And
			                      ? " AND "
			                      : " OR ";
			        }
			    }

				//Append
				if (conditionClause.Length > 0)
				{
				    var clause = conditionClause.ToString();
				    filterClause.Append(filterClause.Length > 0
				                            ? string.Format(" AND ( {0} )", clause)
				                            : string.Format(" {0} ", clause));
				}

			    conditionClause.Clear();

				//make the final whereClause
				whereClause = filterClause.ToString();

				//build sort clause
				var sortClause = "";
				con = "";
				if (sortColumns != null)
				{
					foreach (SortColumn sortColumn in sortColumns)
					{
						var part = sortColumn.FieldName;
						sortClause += con + part;
						if (sortColumn.SortDescending) sortClause += " DESC";
						con = ", ";
					}
				}
				if (string.IsNullOrWhiteSpace(sortClause))
					sortClause = "ID"; //we need some column for the OVER part

				sb = new StringBuilder(string.Format("USE [Imarda360.{0}]; ", database));

				int startNum = 1;
				int endNum = Int32.MaxValue;
				if (offset.HasValue)
					startNum = offset.Value;
				if (startNum < 1) startNum = 1;
				if (limit.HasValue && limit.Value > 0)
					endNum = startNum + limit.Value - 1;

				sb.Append("SELECT numberedData.* FROM (");
				sb.Append(string.Format("SELECT innerSelect.*, ROW_NUMBER() OVER (ORDER BY {0} ) AS RowNum FROM ( {1} ) AS innerSelect ", sortClause, innerSelect));
				if (!string.IsNullOrWhiteSpace(whereClause))
					sb.Append(string.Format(" WHERE ({0}) ", whereClause));
				sb.Append(") AS numberedData");
				sb.Append(string.Format(" WHERE numberedData.RowNum BETWEEN {0} AND {1}", startNum, endNum));


				string query = sb.ToString();
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
			    if (sb != null)
			    {
			        ErrorHandler.ErrorFormat("sql:{0}|ex:{1}", sb, ex);
			    }
				return ErrorHandler.Handle<GetListResponse<T>>(ex, typeof(T));
			}
		}

		#endregion

		#region GenericGetExtentWithCustomSelectCount<T>

		protected SimpleResponse<int> GenericGetExtentWithCustomSelectCount<T>(
			Guid? companyID,
			DateTime? createdAfter,
			DateTime? createdBefore,
			DateTime? modifiedAfter,
			DateTime? modifiedBefore,
			bool? deleted,
			bool? active,
			bool? isTemplate,
			string path,
			Guid? ownerID,
			int? ownerType,
			string innerSelect,
			List<Condition> conditions,
			ConditionLogicalOperator logicalOperator
			)
			where T : FullBusinessEntity, new()
		{
			StringBuilder sb = null;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				Type type = typeof(T);
				string tableName = type.Name;
				string ns = type.Namespace; //e.g. ImardaTrackingBusiness, ImardaCRMBusiness
				string database = _DatabaseNameRegex.Match(ns).Value;

				//build where clause
				var whereClause = "";
				var con = "";
				//base filtering
				string[] filters =
					{
						//companyID.HasValue && companyID.Value != Guid.Empty ?string.Format(" [CompanyID] = '{0}' ", companyID.Value.ToString()) : null,
						createdAfter.HasValue ? string.Format(" [DateCreated] >= '{0}' ", createdAfter.Value.ToString("s")) : null,
						createdBefore.HasValue ? string.Format(" [DateCreated] <= '{0}' ", createdBefore.Value.ToString("s")) : null,
						modifiedAfter.HasValue ? string.Format(" [DateModified] >= '{0}' ", modifiedAfter.Value.ToString("s")) : null,
						modifiedBefore.HasValue ? string.Format(" [DateModified] <= '{0}' ", modifiedBefore.Value.ToString("s")) : null,
						deleted.HasValue ? " [Deleted] = " + (deleted.Value ? '1' : '0') : null,
						active.HasValue ? " [Active] = " + (active.Value ? '1' : '0') : null,
					};
				//these filters are 'AND'd together
				var filterClause = new StringBuilder();
				con = "";
				foreach (var filter in filters)
				{
					if (filter != null)
					{
						filterClause.Append(con).Append(filter);
						con = " AND ";
					}
				}

				//template we need handle differently because we treat null as false 
				if (isTemplate.HasValue)
				{
					if (isTemplate.Value == true)
					{
						filterClause.Append(con).Append(" [IsTemplate] = 1 ");
					}
					else
					{
						filterClause.Append(con).Append(" ([IsTemplate] = 0 OR [IsTemplate] IS NULL) ");
					}
				}

				//Add any further conditions 
				var conditionClause = new StringBuilder();
				con = "";

				if (conditions != null)
				{
					foreach (var condition in conditions)
					{
						var part = condition.GetSQL();
						conditionClause.Append(con).Append(part);
						con = logicalOperator == ConditionLogicalOperator.And ? " AND " : " OR ";
					}
				}

				//Append
				if (conditionClause.Length > 0)
				{
					var clause = conditionClause.ToString();
					filterClause.Append(filterClause.Length > 0
											? string.Format(" AND ( {0} )", clause)
											: string.Format(" {0} ", clause));
				}

				conditionClause.Clear();

				//make the final whereClause
				whereClause = filterClause.ToString();

				sb = new StringBuilder(string.Format("USE [Imarda360.{0}]; ", database));
				sb.Append(string.Format("SELECT COUNT(*) FROM ({0}) AS innerSelect ", innerSelect));
				if (!string.IsNullOrWhiteSpace(whereClause))
					sb.Append(string.Format(" WHERE ({0}) ", whereClause));
				string query = sb.ToString();

				var count = -1;
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					if (dr.Read()) count = Convert.ToInt32(dr[0]);
				}
				return new SimpleResponse<int>(count);
			}
			catch (Exception ex)
			{
				if (sb != null)
				{
					ErrorHandler.ErrorFormat("sql:{0}|ex:{1}", sb, ex);
				}
				return ErrorHandler.Handle<SimpleResponse<int>>(ex);
			}
		}

		#endregion

		#region GenericGetEntityUpdateCount<T>

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetUpdateCountResponse GenericGetEntityUpdateCount<T>(string entityName, DateTime timeStamp, bool rethrow, params Guid[] ids) 
            where T : BusinessEntity, new()
		{
			var response = new GetUpdateCountResponse();
			response.TypeName = entityName;

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

				var args = new object[ids.Length + 1];
				for (int i = 0; i < ids.Length; i++)
				{
					args[i] = ids[i];
				}
				args[args.Length - 1] = ReadyDateForStorage(timeStamp);

				using (IDataReader dr = db.ExecuteDataReader("SPGet" + entityName + "UpdateCount", args))
				{
					if (dr.Read()) response.Count = Convert.ToInt32(dr[0]);
				}
			}
			catch (Exception ex)
			{
				if (rethrow) throw;
				ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetUpdateCountResponse GenericGetEntityUpdateCount<T>(string entityName, DateTime timeStamp,
																																		params Guid[] ids) where T : BusinessEntity, new()
		{
			return GenericGetEntityUpdateCount<T>(entityName, timeStamp, false, ids);
		}

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetUpdateCountResponse GenericGetEntityUpdateCount<T>(DateTime timeStamp, bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityUpdateCount<T>(typeof(T).Name, timeStamp, rethrow, ids);
		}

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetUpdateCountResponse GenericGetEntityUpdateCount<T>(DateTime timeStamp, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityUpdateCount<T>(typeof(T).Name, timeStamp, false, ids);
		}

		#endregion

		#region GenericGetEntityCount<T>

		/// <summary>
		/// Gets number of entities of the given Type
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "Count"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
        ///<param name="ids"></param>
		/// <returns>A Count value of number of records</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetUpdateCountResponse GenericGetEntityCount<T>(string entityName, bool rethrow, params Guid[] ids) where T : BusinessEntity, new()
		{
			var response = new GetUpdateCountResponse();
			response.TypeName = entityName;

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

				var args = new object[ids.Length];
				for (int i = 0; i < ids.Length; i++)
				{
					args[i] = ids[i];
				}

				using (IDataReader dr = db.ExecuteDataReader("SPGet" + entityName + "Count", args))
				{
					if (dr.Read()) response.Count = Convert.ToInt32(dr[0]);
				}
			}
			catch (Exception ex)
			{
				if (rethrow) throw;
				ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}

		/// <summary>
		/// Gets number of entities of the given Type
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "Count"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="ids">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <returns>A Count value of number of records</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetUpdateCountResponse GenericGetEntityCount<T>(string entityName, params Guid[] ids) where T : BusinessEntity, new()
		{
			return GenericGetEntityCount<T>(entityName, false, ids);
		}

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetUpdateCountResponse GenericGetEntityCount<T>(bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityCount<T>(typeof(T).Name, rethrow, ids);
		}

		/// <summary>
		/// Gets number of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "UpdateCount"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// 

		protected GetUpdateCountResponse GenericGetEntityCount<T>(params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityCount<T>(typeof(T).Name, false, ids);
		}

		#endregion

		#region GenericGetEntityListByTimestamp<T>

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "ListByTimeStamp"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="cap"></param>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <param name="includeInactive"></param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetListResponse<T> GenericGetEntityListByTimestamp<T>
			(
			string entityName,
			DateTime timeStamp,
			int cap,
			bool rethrow,
			bool includeInactive,
			params Guid[] ids
			)
			where T : BusinessEntity, new()
		{
		    var storedProc = "null";
            object[] args = null;
            try
			{
                storedProc = "SPGet" + entityName + "ListByTimeStamp";
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				
				args = new object[ids.Length + 3];
				args[0] = includeInactive;
				args[1] = cap;

				for (int i = 0; i < ids.Length; i++)
				{
					args[i + 2] = ids[i];
				}
				args[args.Length - 1] = ReadyDateForStorage(timeStamp);
                using (IDataReader dr = db.ExecuteDataReader(storedProc, args))
				{
					var response = new GetListResponse<T> { List = new List<T>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<T>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
                ErrorHandler.ErrorFormatStoredProc(storedProc, args, ex);
				if (rethrow) throw;
				return ErrorHandler.Handle<GetListResponse<T>>(ex);
			}
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + entityName + "ListByTimeStamp"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="entityName">The Name of the entity</param>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="includeInactive"></param>
		/// <param name="ids">The ids to check</param>
		/// <param name="cap"></param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListByTimestamp<T>(
			string entityName,
			DateTime timeStamp,
			int cap,
			bool includeInactive,
			params Guid[] ids
			) where T : BusinessEntity, new()
		{
			return GenericGetEntityListByTimestamp<T>(entityName, timeStamp, cap, false, includeInactive, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "ListByTimeStamp"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="cap"></param>
		/// <param name="rethrow">
		/// A bool indicating whether to rethrow any Exception (true)
		/// or encapsulate it in the response message (false)
		/// </param>
		/// <param name="includeInactive"></param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		/// <exception cref="Exception">Thrown if an error occurs and rethrow is set to true</exception>
		protected GetListResponse<T> GenericGetEntityListByTimestamp<T>(
			DateTime timeStamp,
			int cap,
			bool rethrow,
			bool includeInactive,
			params Guid[] ids
			) where T : BusinessEntity, new()
		{
			return GenericGetEntityListByTimestamp<T>(typeof(T).Name, timeStamp, cap, rethrow, includeInactive, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids givn by the ids
		/// and have been updated since the time shown in timeStamp.
		/// Will work fine, assuming the database Stored Proceduers are written according to convention:
		/// The SP name is built according to "SPGet: + typeof(T).Name + "ListByTimeStamp"
		/// and the last argument is the time stamp.
		/// </summary>
		/// <typeparam name="T">The Type of the entity to get</typeparam>
		/// <param name="timeStamp">The time stamp to check against</param>
		/// <param name="cap"></param>
		/// <param name="ids">The ids to check</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListByTimestamp<T>(DateTime timeStamp, int cap, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityListByTimestamp<T>(typeof(T).Name, timeStamp, cap, false, ids);
		}

		#endregion

		#region GenericGetEntityListDateRange<T>

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids,
		/// within the date range given by dateFrom and dateTo.
		/// SP name is built according to "SPGet" + entityName + "ListDateRange"
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned</typeparam>
		/// <param name="entityName">The name of the entity in the Stored Procedure</param>
		/// <param name="dateFrom">The minimum date match to return</param>
		/// <param name="dateTo">The maximum date match to return</param>
		/// <param name="ids">A list of GUIDs that must match, in the same order as in the stored procedure</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListDateRange<T>(string entityName, DateTime dateFrom, DateTime dateTo,
																																	params Guid[] ids) where T : BusinessEntity, new()
		{
			return GenericGetEntityListDateRange<T>(entityName, dateFrom, dateTo, false, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids,
		/// within the date range given by dateFrom and dateTo.
		/// SP name is built according to "SPGet" + entityName + "ListDateRange"
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned</typeparam>
		/// <param name="entityName">The name of the entity in the Stored Procedure</param>
		/// <param name="dateFrom">The minimum date match to return</param>
		/// <param name="dateTo">The maximum date match to return</param>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="ids">A list of GUIDs that must match, in the same order as in the stored procedure</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListDateRange<T>(string entityName, DateTime dateFrom, DateTime dateTo,
																																	bool rethrow, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			if (dateFrom.Equals(DateTime.MinValue) & dateTo.Equals(DateTime.MaxValue))
			{
				// No date range. Return all of them
				return GenericGetEntityList<T>(entityName, ids);
			}
			else
			{
				if (dateFrom.Equals(DateTime.MinValue))
				{
					// TODO Correct minimum before sticking into SP to avoid crashes
				}
				if (dateTo.Equals(DateTime.MaxValue))
				{
					// TODO Correct maximum before sticking into SP to avoid crashes
				}
				try
				{
					var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());

					var args = new object[ids.Length + 2];
					for (int i = 0; i < ids.Length; i++)
					{
						args[i] = ids[i];
					}
					args[ids.Length] = dateFrom;
					args[ids.Length + 1] = dateTo;

					using (IDataReader dr = db.ExecuteDataReader("SPGet" + entityName + "ListDateRange", args))
					{
						var response = new GetListResponse<T> { List = new List<T>() };
						while (dr.Read())
						{
							response.List.Add(GetFromData<T>(dr));
						}
						return response;
					}
				}
				catch (Exception ex)
				{
					if (rethrow) throw;
					return ErrorHandler.Handle<GetListResponse<T>>(ex);
				}
			}
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids,
		/// within the date range given by dateFrom and dateTo.
		/// SP name is built according to "SPGet" + typeof(T).Name + "ListDateRange"
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned</typeparam>
		/// <param name="dateFrom">The minimum date match to return</param>
		/// <param name="dateTo">The maximum date match to return</param>
		/// <param name="ids">A list of GUIDs that must match, in the same order as in the stored procedure</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListDateRange<T>(DateTime dateFrom, DateTime dateTo, params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityListDateRange<T>(dateFrom, dateTo, false, ids);
		}

		/// <summary>
		/// Gets a list of entities of the given Type that have the Guids given by the ids,
		/// within the date range given by dateFrom and dateTo.
		/// SP name is built according to "SPGet" + typeof(T).Name + "ListDateRange"
		/// </summary>
		/// <typeparam name="T">The Type of the object to be returned</typeparam>
		/// <param name="dateFrom">The minimum date match to return</param>
		/// <param name="dateTo">The maximum date match to return</param>
		/// <param name="rethrow">Value indicating the action when an exception is encountered. True
		/// to throw the exception to the caller (the caller should handle this), false to
		/// catch the exception and wrap in a business message response.</param>
		/// <param name="ids">A list of GUIDs that must match, in the same order as in the stored procedure</param>
		/// <returns>A list of objects of the given Type</returns>
		protected GetListResponse<T> GenericGetEntityListDateRange<T>(DateTime dateFrom, DateTime dateTo, bool rethrow,
																																	params Guid[] ids)
			where T : BusinessEntity, new()
		{
			return GenericGetEntityListDateRange<T>(typeof(T).Name, dateFrom, dateTo, ids);
		}

		#endregion

		protected GetItemResponse<T> GenericGetBusinessEntity<T>(IDRequest req)
			where T : BusinessEntity, new()
		{
			string entityName = typeof(T).Name;
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				spName = "SPGet" + entityName;
				using (IDataReader dr = db.ExecuteDataReader(spName, req.ID))
				{
					var response = new GetItemResponse<T>();
					if (dr.Read()) response.Item = GetFromData<T>(dr);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<T>>(ex, spName, req);
			}
		}

		#region GenericGetEntity<T>

		protected GetItemResponse<T> GenericGetEntity<T>(IDRequest req)
			where T : FullBusinessEntity, new()
		{
			string entityName = typeof(T).Name;
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				bool includeAttributes = req.HasSome(RetrievalOptions.Attributes);

                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
                string prefix = includeAttributes ? "SPGetX" : "SPGet";
				spName = prefix + entityName;
                //spName = "SPGet" + entityName;  //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
				using (IDataReader dr = db.ExecuteDataReader(spName, req.ID))
				{
					var response = new GetItemResponse<T>();
					if (dr.Read()) response.Item = GetFromData<T>(dr, includeAttributes);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<T>>(ex, spName, req);
			}
		}

        protected GetItemResponse<T> GenericGetCachedEntity<T>(IDRequest req)
            where T : FullBusinessEntity, new()
        {
            string entityName = typeof (T).Name;
            var entity = _EntityCacheManager.GetEntityItemFromEntityCache(entityName, req.ID);
            if (entity != null)
            {
                var response = new GetItemResponse<T>();
                response.Item = (T)entity;
                return response;
            }
            string spName = entityName;
            try
            {
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                bool includeAttributes = req.HasSome(RetrievalOptions.Attributes);

                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
                string prefix = includeAttributes ? "SPGetX" : "SPGet";
                spName = prefix + entityName;
                //spName = "SPGet" + entityName;  //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
                using (IDataReader dr = db.ExecuteDataReader(spName, req.ID))
                {
                    var response = new GetItemResponse<T>();
                    if (dr.Read()) response.Item = GetFromData<T>(dr, includeAttributes);
                    _EntityCacheManager.AddEntityItemToEntityCache(response.Item); 
                    return response;
                }
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<T>>(ex, spName, req);
            }
        }

        //-----------------------------
        /// <summary>
        /// This method is temporary, while we're still using the old format for attributes in code
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProc"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public GetItemResponse<T> GetItem_IncludeAttributes<T>(string storedProc, params object[] args)  //IM-3747 - temp
            where T : FullBusinessEntity, new()
        {
            try
            {
                var response = ImardaDatabase.GetItem<T>(storedProc, args);
                if (response.Item != null) PopulateEntityAttributes(response.Item);    //IM-3747
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<T>>(ex);
            }
        }

        /// <summary>
        /// This method is temporary, while we're still using the old format for attributes in code //IM-3747
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProc"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public GetListResponse<T> GetList_IncludeAttributes<T>(string storedProc, params object[] args)  //IM-3747 - temp
            where T : FullBusinessEntity, new()
        {
            try
            {
                var response = ImardaDatabase.GetList<T>(storedProc, args);
                foreach (var item in response.List)
                {
                    PopulateEntityAttributes(item);    //IM-3747
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<T>>(ex);
            }
        }

        /// <summary>
        /// Temp method to convert new format AttributeValueList into old pipe delimited string - used while we still have code using piped strings
        /// </summary>
        /// <param name="avList"></param>
        /// <returns></returns>
        public string ConvertAttributeValueListToPipedString(List<AttributeValue> avList)   //IM-3747
        {
            var sb = new StringBuilder();
            foreach (var av in avList)
            {
                if (sb.ToString() != "")
                {
                    char lastChar = sb[sb.Length - 1];
                    if (lastChar == '|')
                        sb.Append("|");
                    else
                        sb.Append("||");
                }

                sb.Append(av.VarName + "|");

                string vartype = "";
                if (av.VarType.StartsWith("!"))
                {
                    vartype = av.VarType.Replace("!", "");
                }

                var values = av.Value.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                for (int x=0; x < values.Length; x++)
                {
                    if (x > 0) sb.Append("|");
                    if (vartype != "") sb.Append(vartype + ":");
                    sb.Append(values[x].Trim());
                    if (av.Format != null && av.Format.StartsWith("~") && av.Format.Length > 1)
                    {
                        sb.Append(" " + av.Format);
                    }
                }

                if (values.Length == 0 && vartype.Trim() == "" && (av.Format == null || av.Format.Trim() == ""))
                {
                    sb.Append(EAHelper.GetDefaultValueString(av.VarType[0]));
                }
            }
            return sb.ToString();
        }
        //------------------------------

        protected GetItemResponse<T> GenericGetCachedEntityByParams<T>(string spName, bool rethrow, IDRequest request) 
            where T : FullBusinessEntity, new()
        {
            try
            {
                string entityName = typeof (T).Name;
                var entity = _EntityCacheManager.GetEntityItemFromEntityCacheByParam<T>(entityName, request);
                if (entity != null)
                {
                    var response = new GetItemResponse<T>();
                    response.Item = (T)entity;
                    return response;
                }

                try
                {
                    var paramValues = new List<object>();
                    paramValues.Add(request.ID);
                    foreach (var key in request.Keys)
                    {
                        object paramValue = null;
                        if (request.Get(key, out paramValue))
                        {
                            paramValues.Add(paramValue);
                        }
                    }

                    var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                    using (IDataReader dr = db.ExecuteDataReader(spName, paramValues.ToArray()))
                    {
                        var response = new GetItemResponse<T>();
                        if (dr.Read()) response.Item = GetFromData<T>(dr, false);
                        if (response.Item != null)
                            _EntityCacheManager.AddEntityItemToEntityCache(response.Item);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHandler.Handle<GetItemResponse<T>>(ex, spName, request);
                }
            }
            catch (Exception ex)
            {
                if (rethrow)
                    throw;
                else
                {
                    return null;  //ErrorHandler.Handle<GetListResponse<T>>(ex, spName, parameters);
                }
            }
        }

        protected GetListResponse<T> GenericGetCachedEntityListByParams<T>(string spName, bool rethrow, IDRequest request)
            where T : FullBusinessEntity, new()
        {
            try
            {
                string entityName = typeof(T).Name;
                var entityList = _EntityCacheManager.GetEntityListFromEntityCacheByParam<T>(entityName, request);
                if (entityList != null)
                {
                    var response = new GetListResponse<T>();
                    response.List = entityList.Cast<T>().ToList();
                    return response;
                }

                try
                {
                    var paramValues = new List<object>();
                    paramValues.Add(request.ID);
                    foreach (var key in request.Keys)
                    {
                        object paramValue = null;
                        if (request.Get(key, out paramValue))
                        {
                            paramValues.Add(paramValue);
                        }
                    }

                    var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                    using (IDataReader dr = db.ExecuteDataReader(spName, paramValues.ToArray()))
                    {
                        var response = new GetListResponse<T> { List = new List<T>() };
                        while (dr.Read())
                        {
                            response.List.Add(GetFromData<T>(dr));
                        }

                        if (response.List.Count > 0)
                            _EntityCacheManager.AddEntityItemListToEntityCache(entityName, response.List.Cast<FullBusinessEntity>().ToList());
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHandler.Handle<GetListResponse<T>>(ex, spName, request);
                }
            }
            catch (Exception ex)
            {
                if (rethrow)
                    throw;
                else
                {
                    return null;  //ErrorHandler.Handle<GetListResponse<T>>(ex, spName, parameters);
                }
            }
        }


		/// <summary>
		///  Calls an SP named SPGetEntityByParamID - supports SPGetX
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="req">ID to select</param>
		/// <param name="paramId">Name of the column containing ID to select</param>
		/// <returns></returns>
		protected GetItemResponse<T> GenericGetEntity<T>(IDRequest req, string paramId)
	where T : FullBusinessEntity, new()
		{
			string entityName = typeof(T).Name;
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				bool includeAttributes = req.HasSome(RetrievalOptions.Attributes);

                // We will remove EntityAttributes from Sps, but still allow the "x" version to be called
                // in case it's different in other ways, from the non-x version //IM-3747
                string prefix = includeAttributes ? "SPGetX" : "SPGet";
				spName = prefix + entityName + "By" + paramId;
                //spName = "SPGet" + entityName + "By" + paramId; //IM-3747 - we no longer use EntityAttributes table and "GetFromData" below will add in attributes if needed
				using (IDataReader dr = db.ExecuteDataReader(spName, req.ID))
				{
					var response = new GetItemResponse<T>();
					if (dr.Read()) response.Item = GetFromData<T>(dr, includeAttributes);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<T>>(ex, spName, req);
			}
		}

		#endregion

		#region GenericSaveEntity

        protected BusinessMessageResponse GenericSaveEntity<T>(object[] properties)
            where T : FullBusinessEntity, new()
        {
            BusinessMessageResponse resp1 = GenericSaveEntity<T>(typeof(T).Name, properties);
            return resp1;
        }

        //TEMP DJ - temp method to allow build. Must change all methods that call here, to use new method below, with "out" of ad and av lists
        // (if they have attributes to save) (so far I've done tracking, vehcile, crm, jobs and notification services)
        protected BusinessMessageResponse GenericSaveEntity<T>(Guid companyID, EntityAttributes ea, object[] properties)
            where T : FullBusinessEntity, new()
        {
            return GenericSaveEntity<T>(typeof(T).Name, properties);
        }

        protected BusinessMessageResponse GenericSaveEntity<T>(Guid companyID, EntityAttributes ea, out List<AttributeDefinition> adList, out List<AttributeValue> avList, object[] properties)
			where T : FullBusinessEntity, new()
		{
            adList = new List<AttributeDefinition>();
            avList = new List<AttributeValue>();

			BusinessMessageResponse resp1 = GenericSaveEntity<T>(typeof(T).Name, properties);
			if (resp1.Status && ea != null && ea.HasAttributes)
			{
                return SaveEntityAttributesToAttributingCache<T>(companyID, (Guid)properties[0], ea, out adList, out avList);   //IM-3747  //TEMPDJ
			}
			return resp1;
		}
        //protected BusinessMessageResponse GenericSaveCachedEntity<T>(FullBusinessEntity entity, EntityAttributes ea, object[] properties)
        //    where T : FullBusinessEntity, new()
        //{
        //    BusinessMessageResponse resp1 = GenericSaveCachedEntity<T>(entity, typeof(T).Name, properties);
        //    if (resp1.Status && ea != null && ea.HasAttributes)
        //    {
        //        return SaveEntityAttributes<T>((Guid)properties[0], ea.UpdateAttributeString());
        //    }
        //    return resp1;
        //}

        protected BusinessMessageResponse SaveEntityAttributes<T>(Guid companyID, Guid id, EntityAttributes ea, out List<AttributeDefinition> adList, out List<AttributeValue> avList)  //IM-3747
			where T : FullBusinessEntity, new()
		{
            adList = new List<AttributeDefinition>();
            avList = new List<AttributeValue>();
			try
			{
                return SaveEntityAttributesToAttributingCache<T>(companyID, id, ea, out adList, out avList);   //IM-3747  //TEMPDJ
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex, id, ea.GetAttributes());
			}
		}

        /// <summary>
        /// This method is only for saving a new AttributeValue row for an existing AttributeDefinition.
        /// (if the attribute definition is new, don't use this method)
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <param name="attributeKey"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool SaveNewAttributeToAttributingCache(Guid companyID, EntityAttributes ea, string entityName, Guid id, string key, string value, out List<AttributeDefinition> adList, out List<AttributeValue> avList)
        {
            adList = new List<AttributeDefinition>();
            avList = new List<AttributeValue>();

            AttributeDefinition ad = _AttributeCacheManager.GetEntityItemFromAttributeDefinitionCache(entityName, companyID, key);
            if (ad == null)     // ok, maybe the attribute defintion is system attribute? 
                ad = _AttributeCacheManager.GetEntityItemFromAttributeDefinitionCache(entityName, new Guid("11111111-1111-1111-1111-111111111111"), key);
            
            if (ad == null)
            {
                // ok the definition doesn't exit. Most likely scenario is that in conversion from old attributing DB structure, to the new one,
                // this attribute was not defined in IXLParameter and so never got moved to AttributeDefinition. This is mostly (wholly?) due to
                // the use of hard-coded attributes in classes such as UnitAttributes. These types of attributes are NOT custom ones for a company and
                // are (I believe) not normally visible in the UI. They are system generated and maintained.
                // So create an AttributeDefinition for the company.
                ad = new AttributeDefinition();
                ad.ID = Guid.NewGuid();
                ad.CompanyID = companyID;
                ad.DateCreated = DateTime.UtcNow;
                ad.DateModified = ad.DateCreated;
                ad.Active = true;
                ad.Deleted = false;
                ad.VarName = key;
                ad.FriendlyName = key;
                ad.Description = key;
                ad.GroupID = Guid.Empty;
                ad.VarType = "$";
                ad.Format = "";
                ad.CaptureHistory = false;  // assume no history needed
                ad.Viewable = true;        // should I assume can't be viewed on UI, as this is likely a system attribute, hard-coded in i360 code
                ad.EntityTypeName = entityName;
                _AttributeCacheManager.AddEntityItemToAttributeDefinitionCache(entityName, ad);

                adList.Add(ad);     // populate to tell calling service that this needs committing to attributing database
            }

            // Now create an AttributeValue and commit to cache
            var attributeValue = new AttributeValue();
            attributeValue.ID = Guid.NewGuid();
            attributeValue.DateModified = DateTime.UtcNow;
            attributeValue.AttributeID = ad.ID;
            attributeValue.EntityID = id;
            attributeValue.Value = value;
            attributeValue.VarName = ad.VarName;
            attributeValue.FriendlyName = ad.FriendlyName;
            attributeValue.Description = ad.Description;
            attributeValue.GroupID = ad.GroupID;
            attributeValue.VarType = ad.VarType;
            attributeValue.Format = ad.Format;
            attributeValue.CaptureHistory = ad.CaptureHistory;
            attributeValue.Viewable = ad.Viewable;
            attributeValue.EntityTypeName = ad.EntityTypeName;
            _AttributeCacheManager.AddEntityItemToAttributeValueCache(entityName, attributeValue);

            avList.Add(attributeValue);     // populate to tell calling service that this needs committing to attributing database
            return true;
        }

        protected AttributeDefinition GetAttributeDefinition(Guid companyID, string entityName, string key)
        {
            var ad = _AttributeCacheManager.GetEntityItemFromAttributeDefinitionCache(entityName, companyID, key);
            if (ad == null)
                _AttributeCacheManager.GetEntityItemFromAttributeDefinitionCache(entityName, new Guid("11111111-1111-1111-1111-111111111111"), key);
            return ad;
        }

        protected List<AttributeDefinition> GetAttributeDefinitionListViaCompanyAndEntityType(Guid companyID, string entityName)
        {
            var adList = _AttributeCacheManager.GetEntityListFromAttributeDefinitionCache(entityName, companyID);
            return adList;
        }

        protected bool DeleteAttributeDefinitionAndValues(Guid companyID, string entityName, string key)
        {
            _AttributeCacheManager.DeleteEntityItemFromAttributeDefinitionCache(companyID, entityName, key);
            return _AttributeCacheManager.DeleteAttributeValuesFromAttributeValueCache(companyID, entityName, key);
        }
        /// <summary>
        /// TEMP DJ - temporary method to persist new attibutes to AttributeDefinition, when they afre being updated to IXLParameter.
        /// Eventually we'll have time to remove all the IXL tables and only save to AttributeDefintion.    //IM-3747
        /// </summary>
        /// <returns></returns>
        protected bool SaveAttributeDefinitionEntity(string entityName, AttributeDefinition ad, bool newDefinition)
        {
            _AttributeCacheManager.AddEntityItemToAttributeDefinitionCache(entityName, ad);

            if (!newDefinition)
            {
                List<AttributeValue> avList = _AttributeCacheManager.GetListFromAttributeValueCacheByAttributeID(entityName, ad);
                if (avList == null) return true;
                foreach (var av in avList)
                {
                    // Update AttributeValue and write back to cache
                    // NOTE - no need to write to DB, as all these values are only stored in AttributeDefintion on the DB
                    av.FriendlyName = ad.FriendlyName;
                    av.Description = ad.Description;
                    av.GroupID = ad.GroupID;
                    av.VarType = ad.VarType;
                    av.Format = ad.Format;
                    av.CaptureHistory = ad.CaptureHistory;
                    av.Viewable = ad.Viewable;
                    av.DateModified = DateTime.UtcNow;
                    //_AttributeCacheManager.AddEntityItemToAttributeValueCache(entityName, av);
                    var avList2 = new List<AttributeValue>();
                    avList2.Add(av);
                    _AttributeCacheManager.ChangeEntityItemListToAttributeValueCache(entityName, av.EntityID, avList2);
                }
                

            }
            return true;
        }

        // TEMPDJ / IM-3747 - temporary method to enable saving of old format EntityAttributes to new AttributeValue table in new Attributing database
        protected BusinessMessageResponse SaveEntityAttributesToAttributingCache<T>(Guid companyID, Guid id, EntityAttributes ea, out List<AttributeDefinition> adList, out List<AttributeValue> avList)
            where T : FullBusinessEntity, new()
        {
            adList = new List<AttributeDefinition>();
            avList = new List<AttributeValue>();

            try
            {
                // Get current cached attributes for this entity
                string entityName = typeof(T).Name;
                var attributeValueList = _AttributeCacheManager.GetEntityAttributeListFromAttributeValueCache(entityName, id);

                if (attributeValueList == null || attributeValueList.Count == 0)
                {
                    // We have NO attribute values in the cache for this entity and id (so it won't be on DB either).
                    // This means we're dealing with a new entity, recently created. There should be attribute definitions present,
                    // which we can use to build up the necessary attribute value rows in the cache and commit them to the DB.
                    foreach (string key in ea.Map.Keys)
                    {
                        string rawString = "";
                        if (ea.Map[key] is String[])
                        {
                            var arr = (string[])ea.Map[key];
                            rawString = string.Join("|", arr);
                        }
                        else
                        {
                            rawString = (string) ea.Map[key];
                        }
                        var newValue = CorrectAttributeValue(rawString);
                        SaveNewAttributeToAttributingCache(companyID, ea, entityName, id, key, newValue, out adList, out avList);
                    }
                    return new BusinessMessageResponse();
                }

                // ok, some or all attributes have been found for the entity - process the change in value. If a new attribute, then create it.
                var attributeValuesChanged = new List<AttributeValue>();
                foreach (string key in ea.Map.Keys)
                {
                    string lowerKey = key.ToLower().Trim();
                    if (lowerKey == "") 
                        continue;

                    // Can be array, so need to test and fix if it is
                    var newValue = ea.Map[key];
                    string newStringValue = "";
                    if (newValue is Array)
                    {
                        string[] stringArray = ((IEnumerable) newValue).Cast<object>()
                            .Select(x => x.ToString())
                            .ToArray();
                        newStringValue = String.Join("|", stringArray);
                    }
                    else
                        newStringValue = (string) newValue;

                    newStringValue = CorrectAttributeValue(newStringValue);

                    var attributeValue = attributeValueList.FirstOrDefault(s => (s.VarName.ToLower() == lowerKey || s.FriendlyName.ToLower() == lowerKey));
                    if (attributeValue == null) // there is no attribute value (and perhaps no definition?)
                    {
                        SaveNewAttributeToAttributingCache(companyID, ea, entityName, id, key, newStringValue, out adList, out avList);
                    }
                    else
                    {
                        if (newStringValue != attributeValue.Value)
                        {
                            // attribute value has changed, so we must update our cache and the DB
                            attributeValue.PrevValue = attributeValue.Value;
                            attributeValue.PrevDateModified = attributeValue.DateModified;
                            attributeValue.Value = newStringValue;
                            attributeValuesChanged.Add(attributeValue);
                        }
                    }
                }

                if (attributeValuesChanged.Count > 0)
                {
                    // We have some attributes to update in cache and in DB
                    // Update cache...
                    _AttributeCacheManager.ChangeEntityItemListToAttributeValueCache(entityName, id, attributeValuesChanged);

                    avList.AddRange(attributeValuesChanged);     // populate to tell calling service that this needs committing to attributing database
                }
                return new BusinessMessageResponse();
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex, id, "");  //attributeString);
            }
        }

        private string CorrectAttributeValue(string attrValue)
        {
            attrValue = attrValue.Replace("Length:", "");
            attrValue = attrValue.Replace("Volume:", "");
            attrValue = attrValue.Replace("Duration:", "");
            attrValue = attrValue.Replace("Voltage:", "");
            attrValue = attrValue.Replace("Speed:", "");
            attrValue = attrValue.Replace("Angle:", "");
            attrValue = attrValue.Replace("Density:", "");
            attrValue = attrValue.Replace("Mass:", "");
            attrValue = attrValue.Replace("FuelEfficiency:", "");
            attrValue = attrValue.Replace("Temperature:", "");

            attrValue = attrValue.Replace("~brg", "");
            attrValue = attrValue.Replace("~dist", "");
            attrValue = attrValue.Replace("~lat", "");
            attrValue = attrValue.Replace("~lon", "");

            return attrValue.Trim();
        }

        public SimpleResponse<string> GetAttributes<T>(Guid id)
        {
            // IM-3747
            try
            {
                string entityName = typeof(T).Name;
                var attributeValueList = _AttributeCacheManager.GetEntityAttributeListFromAttributeValueCache(entityName, id);
                if (attributeValueList != null && attributeValueList.Count > 0)
                {
                    // take new attribute format and change into old format of a pipe delimited string and add to response object
                    string pipedString = ConvertAttributeValueListToPipedString(attributeValueList);
                    return new SimpleResponse<string>(pipedString);
                }
                return new SimpleResponse<string>(string.Empty);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<string>(ErrorHandler.Handle(ex, typeof(T), id));
            }

            // Old code follows...      //IM-3747
            //try
            //{
            //    var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
            //    using (IDataReader dr = db.ExecuteDataReader("SPGetEntityAttributes", id))
            //    {
            //        if (dr.Read() && BaseEntity.HasColumn(dr, "Attributes"))
            //        {
            //            var attr = dr["Attributes"] as string;
            //            return new SimpleResponse<string>(attr);
            //        }
            //        return new SimpleResponse<string>(string.Empty);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return new SimpleResponse<string>(ErrorHandler.Handle(ex, typeof(T), id));
            //}
        }

        /// <summary>
        /// Get the attributes of the entity with the given ID. Do not get the entity itself.
        /// Use this call to retrieve attributes for a entity that you have already loaded.
        /// </summary>
        /// <param name="id">the entity ID</param>
        /// <returns>a simple response with the attribute string stored in Value </returns>
        /// <remarks>The caller should wrap the attributes string in a EntityAttributes object before using it.
        /// Then it should be assigned to the actual object to make sure it gets saved if changes were made.</remarks>
        /// <example>
        /// <code>
        /// var resp = channel.GetAttributes&lt;Driver&gt;(driver.ID);
        /// driver.Attributes = new EntityAttributes(resp.Value);
        /// </code>
        /// </example>
        /// <summary>
        /// Saves an entity with the given properties to the database.
        /// The SP name to call is built from entityName.
        /// Only works for the new style of Save, where Adding a new one / saving an existing one are
        /// handled by the same SP.
        /// 
        /// Default to wraping exceptions and returning the response.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected BusinessMessageResponse GenericSaveEntity<T>(string entityName, params object[] properties)
			where T : BusinessEntity, new()
		{
			string spName = entityName;
			try
			{
			    ImardaDatabase db;
			    if (entityName == "AttributeValue" || entityName == "AttributeDefinition")                                             //IM-3747
                    db = ImardaDatabase.CreateDatabase("ImardaAttributingBusinessInterface");   //IM-3747
                else
    				db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				spName = "SPSave" + entityName;
				db.ExecuteNonQuery(spName, properties);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex, spName, properties);
			}
		}

        protected BusinessMessageResponse GenericSaveCachedEntity<T>(FullBusinessEntity entity, string entityName, params object[] properties)
            where T : BusinessEntity, new()
        {
            string spName = entityName;
            try
            {
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                spName = "SPSave" + entityName;
                db.ExecuteNonQuery(spName, properties);
                _EntityCacheManager.AddEntityItemToEntityCache(entity);      //& IM-5178
                return new BusinessMessageResponse();
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex, spName, properties);
            }
        }
		#endregion

		#region GenericDeleteEntity

		/// <summary>
		/// Deletes a single generic entity from the database.
		/// Pass in any number of defining fields, in the order they are to be passed to the database.
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="definingFields"></param>
		/// <returns></returns>
		protected BusinessMessageResponse GenericDeleteEntity<T>(string entityName, params object[] definingFields)
			where T : BusinessEntity, new()
		{
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				spName = "SPDelete" + entityName;
				db.ExecuteNonQuery(spName, definingFields);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex, spName, definingFields);
			}
		}
        protected BusinessMessageResponse GenericDeleteCachedEntity<T>(string entityName, params object[] definingFields)
            where T : BusinessEntity, new()
        {
            string spName = entityName;
            try
            {
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
                spName = "SPDelete" + entityName;
                db.ExecuteNonQuery(spName, definingFields);
                _EntityCacheManager.DeleteEntityItemFromEntityCache(entityName, (Guid)definingFields[0]);      //& IM-5178
                return new BusinessMessageResponse();
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex, spName, definingFields);
            }
        }

		#endregion

		#region GenericDeleteEntityList

		/// <summary>
		/// Deletes a list of entities from the database.
		/// Pass in any number of defining fields, in the order they are to be passed to the database.
		/// 
		/// Default to wraping exceptions and returning the response.
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="definingFields"></param>
		/// <returns></returns>
		protected BusinessMessageResponse GenericDeleteEntityList<T>(string entityName, params object[] definingFields)
			where T : BusinessEntity, new()
		{
			string spName = entityName;
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<T>());
				spName = "SPDelete" + entityName + "List";
				db.ExecuteNonQuery(spName, definingFields);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex, spName, definingFields);
			}
		}

		#endregion

		#region Date Methods

		/// <summary>
		/// Converts any DateTime.MinValue to January 1, 1753, 12:00:00am.
		/// Leaves other dates untouched.
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static DateTime ReadyDateForStorage(DateTime datetime)
		{
			if (datetime <= SqlDateTime.MinValue.Value)
			{
				return new DateTime(1753, 1, 1, 0, 0, 0);
			}
			return datetime;
		}

		//public static DateTime ReadySmallDateForStorage(DateTime datetime)
		//{
		//  if (datetime <= SqlDateTime.MinValue.Value)
		//  {
		//    return new DateTime(1900, 1, 1, 0, 0, 0);
		//  }
		//  return datetime;
		//}

		public static object ReadyDateForStorage(DateTime? dt)
		{
			if (dt.HasValue) return ReadyDateForStorage(dt.Value);
			else return null;
		}

		/// <summary>
		/// Converts any DateTime set to January 1, 1753, 12:00:00am to
		/// DateTime.MinValue. Leaves other dates untouched.
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static DateTime ReadyDateForTransport(DateTime datetime)
		{
			if (datetime <= new DateTime(1753, 1, 3, 0, 0, 0)) return DateTime.MinValue;
			else return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
		}


		#endregion

		#region Get Server Datetime

		public DateTime CurrentServerTime()
		{
			return DateTime.UtcNow;
		}

		#endregion

	}
}