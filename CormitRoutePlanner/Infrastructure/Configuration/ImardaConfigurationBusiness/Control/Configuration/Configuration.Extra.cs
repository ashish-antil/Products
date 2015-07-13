using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FernBusinessBase;
using Imarda.Lib.MVVM.Extensions;
using Imarda.Logging;
using Imarda360.Infrastructure.ConfigurationService;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;
using FernBusinessBase.Errors;
using Newtonsoft.Json;
using Imarda.Lib;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<Configuration> GetConfigurationByUID(IDRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Configuration>());
				Guid uid = request.ID;
				using (IDataReader dr = db.ExecuteDataReader("SPGetConfiguration", uid))
				{
					var response = new GetItemResponse<Configuration>();
					if (dr.Read()) response.Item = GetFromData<Configuration>(dr, false);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Configuration>>(ex);
			}
		}

		public BusinessMessageResponse RemoveFromCache(ConfigListRequest request)
		{
			try
			{
				var context = new BaseContext(request.GetLevels());
				int count = 0;
				if (request.IDs == null)
				{
					count = ConfigCache.Instance.RemoveMatch(new ConfigKey(Guid.Empty, context));
				}
				else
				{
					foreach (Guid id in request.IDs)
					{
						ConfigKey ckey = new ConfigKey(id, context);
						count += ConfigCache.Instance.RemoveMatch(ckey);
					}
				}
				return new BusinessMessageResponse() { Status = true, StatusMessage = count.ToString() };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}




		#region Save Configuration
		public BusinessMessageResponse SaveConfigurationByUID(SaveRequest<Configuration> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Configuration entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]{			
					entity.ID,
					entity.Active,
					entity.Deleted,
					entity.DateCreated,
					entity.DateModified = DateTime.UtcNow,
					entity.CompanyID,
					entity.UserID,
					entity.Combine,
					entity.ValueType,
					entity.VersionValue,
					entity.Notes,
					entity.Hierarchy,
					entity.Level1 == Guid.Empty ? null : (object)entity.Level1,
					entity.Level2 == Guid.Empty ? null : (object)entity.Level2,
					entity.Level3 == Guid.Empty ? null : (object)entity.Level3,
					entity.Level4 == Guid.Empty ? null : (object)entity.Level4,
					entity.Level5 == Guid.Empty ? null : (object)entity.Level5,
					entity.UID
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				ConfigCache.Instance.RemoveUID(entity.UID);
                response = GenericSaveEntity<Configuration>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete Configuration


		public BusinessMessageResponse DeleteConfiguration(IDRequest request)
		{
			try
			{
				//TODO remove from cache.
				//return GenericDeleteEntity<Configuration>("Configuration", request.ID);
				throw new NotImplementedException("DeleteConfiguration is not yet implemented");
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion

		public BusinessMessageResponse DeleteConfigurationByUID(IDRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Configuration>());
				Guid uid = request.ID;
				db.ExecuteNonQuery("DeleteConfigurationByUID", uid);
				ConfigCache.Instance.RemoveUID(uid);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

		}

		public BusinessMessageResponse UpdateConfigValue(SaveRequest<ConfigValue> request)
		{
			try
			{
				ConfigValue val = request.Item;
				string name = Util.GetConnName<Configuration>();
				var db = ImardaDatabase.CreateDatabase(name);
				int n = db.ExecuteNonQuery("UpdateConfiguration", val.UID, val.Value);
				if (n == 1)
				{
					ConfigCache.Instance.RemoveUID(val.UID);
					return new BusinessMessageResponse();
				}
				else throw new Exception(string.Format("Could not update Config UID {0}", val.UID));
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		/// <summary>
		/// Get a single configuration item value.
		/// </summary>
		/// <param name="request">request parameters</param>
		/// <returns>the value wrapped in a ConfigValue object</returns>
		public GetItemResponse<ConfigValue> GetConfigValue(ConfigRequest request)
		{
			try
			{
				var response = new GetItemResponse<ConfigValue>();
				object appParam = request.AppParameter;

				bool cacheable = (appParam == null) && !request.IgnoreCache;
				using (ConfigServiceContext search = new ConfigServiceContext(this, cacheable, request.GetLevels()))
				{
					ConfigItem item = FindItem(request.ID);
					if (item == null)
					{
						throw new ArgumentException("Config Item ID " + request.ID + " not found");
					}
					else
					{
						if (item.Parameter == CfgSystem.First) cacheable = false;
						else item.Parameter = appParam;

						var result = item.CalcValue();

						response.Item = new ConfigValue
						{
							Type = result.ValueType,
							Value = result.VersionValue,
							UID = result.UID
						};

						if (cacheable) Encache(item.ID, search.Context, response.Item);
					}
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ConfigValue>>(ex);
			}
		}

		public GetListResponse<ConfigValue> GetConfigValueList(ConfigListRequest request)
		{
			try
			{
				object appParam = request.AppParameter;
				bool cacheable = (appParam == null) && !request.IgnoreCache;

				var errorList = new List<string>();
				using (ConfigServiceContext search = new ConfigServiceContext(this, cacheable, request.GetLevels()))
				{
					Guid[] ids = request.IDs;
					var results = new ConfigValue[ids.Length];
					for (int i = 0; i < ids.Length; i++)
					{
						Guid id = ids[i];
						ConfigItem item = FindItem(ids[i]);
						if (item == null)
						{
							errorList.Add(id.ToString());
						}
						else
						{
							if (item.Parameter == CfgSystem.First) cacheable = false;
							else item.Parameter = appParam;

							var result = item.CalcValue();
							var value = results[i] = new ConfigValue
							{
								Type = result.ValueType,
								Value = result.VersionValue,
								UID = result.UID
							};
							if (cacheable) Encache(item.ID, search.Context, value);
						}
					}
					if (errorList.Count > 0) throw new ArgumentException("Not found: " + string.Join("|", errorList.ToArray()));
					return new GetListResponse<ConfigValue>() { Status = true, List = new List<ConfigValue>(results) };
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ConfigValue>>(ex);
			}
		}

        public SimpleResponse<List<ConfigValueAndDescr>> GetConfigValueDescrByIDs(ConfigListRequest request)
        {
            var configItems = GetItems(request.ID, request.IDs).Select(c =>
            {
                var calcValue = c.CalcValue();
                var val = JsonConvert.DeserializeObject<ConfigValueAndDescr>(calcValue.VersionValue.ToString());
                val.ID = c.ID;
                return val;
            });
            return new SimpleResponse<List<ConfigValueAndDescr>>(configItems.ToList()) { Status = true };
        }


		private void Encache(Guid id, BaseContext ctx, ConfigValue value)
		{
			var key = new ConfigKey(id, ctx);
			ConfigCache.Instance[key] = value;
		}



		#region IConfigData Members


		/// <summary>
		/// Find the item in the cache of database. Executes within a ConfigContext that
		/// contains the IConfigData and hierarchy levels.
		/// </summary>
		/// <param name="itemID">identifies the item to find</param>
		/// <returns>an object that contains an ordered list of configuration versions that matched</returns>
		public ConfigItem FindItem(Guid itemID)
		{
			try
			{
				// get the context parameters

				ConfigServiceContext search = ConfigServiceContext.Get();
				BaseContext ctx = search.Context;
				ConfigItem item;

				if (search.Cacheable)
				{
					// first look in the cache

					var key = new ConfigKey(itemID, ctx);
					ConfigValue cachedValue = ConfigCache.Instance[key];
					if (cachedValue != null)
					{
						var version = ConfigItemVersion.Create(cachedValue.Type, cachedValue.Value, false, cachedValue.UID);
						item = new ConfigItem
						{
							ID = itemID,
							Versions = new ConfigItemVersion[] { version },
							Parameter = CfgSystem.First
						};

						return item;
					}
				}

				// look in the database

				string name = Util.GetConnName<Configuration>();
				var db = ImardaDatabase.CreateDatabase(name);

				int n = ctx.Depth;

				object[] args = new object[7];
				args[0] = itemID;
				args[1] = 0; // hierarchy
				for (int i = 0; i < n; i++) args[i + 2] = ctx[i];

				using (IDataReader dr = db.ExecuteDataReader("GetConfiguration", args))
				{
					var versions = new List<ConfigItemVersion>();

					item = new ConfigItem { ID = itemID };
					bool combine = true;
					while (combine && dr.Read())
					{
						var c = GetFromData<Configuration>(dr);
						var version = ConfigItemVersion.Parse(c.ValueType, c.VersionValue, c.Combine, c.UID);
						versions.Add(version);
						combine = version.Combine; // =optimization: we don't need to construct more versions unless combinable
					}
					item.Versions = versions.ToArray();

					return item;
				}


			}
			catch
			{
				// do nothing
				#region Trace
#if DEBUG
                ErrorLogger.Trace(Imarda.Logging.TraceLogs.ApnTrace01, string.Format("Exception for item ID: {0}", itemID));
#endif
				#endregion Trace
			}
			return null;
		}

        /// <summary>
        /// Gets ConfigItems for a specified item IDs
        /// </summary>
        public IEnumerable<ConfigItem> GetItems(Guid companyId, ICollection<Guid> itemIDs)
	    {
	        var idTable = new DataTable("IDs");
            idTable.Columns.Add("ID", typeof(Guid));
            itemIDs.ForEach(c => idTable.Rows.Add(c));

            var result = new List<ConfigItem>();
            var name = Util.GetConnName<Configuration>();
            var db = ImardaDatabase.CreateDatabase(name);
            using (var cmd = db.GetDbCommand("SPGetConfigurationByIDs"))
            {
                cmd.Parameters.AddRange(new[]
                {
                    new SqlParameter("@IDs", SqlDbType.Structured) {TypeName = "dbo.uniqueidentifiers", Value = idTable},
                    new SqlParameter("@CompanyID", SqlDbType.UniqueIdentifier) {Value = companyId},
                    new SqlParameter("@hierarchy", SqlDbType.Int) {Value = 0}
                });
                using (var dr = db.ExecuteDataReader(cmd))
                {
                    while (dr.Read())
                    {
                        var c = GetFromData<Configuration>(dr);
                        var version = ConfigItemVersion.Parse(c.ValueType, c.VersionValue, c.Combine, c.UID);
                        var versions = new List<ConfigItemVersion> {version};

                        var item = new ConfigItem {ID = dr.GetGuid(0), Versions = versions.ToArray()};
                        result.Add(item);
                    }
                    return result;
                }
            }
	    }

		public ConfigItemVersion FindDefaultItemVersion(Guid itemID)
		{
			using (new ConfigServiceContext(this, false, new Guid[0]))
			{
				ConfigItem item = FindItem(itemID);
				return item.Versions[0];
			}
		}

		public int AddDefaultItem(Guid itemID, object data, bool update)
		{
			//TODO add the default item to the database.
			throw new NotImplementedException();
		}

		#endregion
	}
}

