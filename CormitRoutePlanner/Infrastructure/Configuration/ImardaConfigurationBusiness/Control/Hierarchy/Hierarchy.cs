using System;
using System.Collections.Generic;
using System.Data;
using FernBusinessBase;
using Imarda.Lib;
using Imarda360.Infrastructure.ConfigurationService;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;
using FernBusinessBase.Errors;

//  
//   _          _            _            _           _
//  | |_ ___   | |__   ___  | |_ ___  ___| |_ ___  __| |
//  | __/ _ \  | '_ \ / _ \ | __/ _ \/ __| __/ _ \/ _` |
//  | || (_) | | |_) |  __/ | ||  __/\__ \ ||  __/ (_| |
//   \__\___/  |_.__/ \___|  \__\___||___/\__\___|\__,_|
//													  
//  not yet tested -MV


namespace ImardaConfigurationBusiness
{
	/// <summary>
	/// Part of the class implements a generalized hierarchy manager, with '
	/// functions to add and remove hierarchical items.
	/// </summary>
	partial class ImardaConfiguration
	{
		public SimpleResponse<int> SaveHierarchy(ConfigRequest req)
		{
			try
			{
				int result;
				object value = req.AppParameter;
				req.AppParameter = null;

				var resp = GetHierarchy(req);
				ErrorHandler.Check(resp);

				// Example:
				// array of existing Configuration objects:
				// 
				//	L1 L2 L3 L4 L5
				// 0  -  -  -  -  -
				// 1  x1 -  -  -  -
				// 2  x1 x2 -  -  -	 existingLevel=2
				// 
				// request new
				// 
				//	[0] [1] [2] [3]
				//	x1  x2  n3  n4	depth=4
				// 
				// create:
				// 
				//	 L1 L2 L3 L4 L5
				// i=3 x1 x2 n3 -  -
				// i=4 x1 x2 n3 n4 -
				// 

				var list = resp.List;
				int existingLevel = list.Count - 1;
				int depth = req.Depth;

				if (existingLevel == depth)
				{
					if (value == null)
					{
						var resp1 = GetChildrenCount(req);
						ErrorHandler.Check(resp1);
						if (resp1.Value == 0)
						{
							// no child nodes: can delete
							var resp3 = DeleteConfigurationByUID(new IDRequest(list[existingLevel].UID));
							ErrorHandler.Check(resp3);
							result = -1;
						}
						else
						{
							// has child nodes, can not delete: do nothing
							result = 0;
						}
					}
					else
					{
						Configuration c = list[existingLevel];
						c.VersionValue = ConfigItemVersion.Create(req.ValueType, value, false, Guid.Empty).ToString();
						var resp1 = SaveConfigurationByUID(new SaveRequest<Configuration>(c));
						ErrorHandler.Check(resp1);
						result = 1;
					}
				}
				else // exisitingLevel < depth
				{
					if (value == null)
					{
						result = 0;
					}
					else
					{
						Guid[] levels = new Guid[5];
						Array.Copy(req.GetLevels(), levels, depth);
						string sValue = ConfigItemVersion.Create(req.ValueType, value, false, Guid.Empty).ToString();
						result = 0;
						for (int i = existingLevel + 1; i <= depth; i++)
						{
							bool combine = i != 0 && req.Combine;
							var c = new Configuration
							{
								ID = req.ID,
								Combine = combine,
								ValueType = req.ValueType,
                                Notes = req.Notes,
								VersionValue = combine ? string.Empty : sValue,
								Level1 = i >= 1 ? levels[0] : Guid.Empty,
								Level2 = i >= 2 ? levels[1] : Guid.Empty,
								Level3 = i >= 3 ? levels[2] : Guid.Empty,
								Level4 = i >= 4 ? levels[3] : Guid.Empty,
								Level5 = i >= 5 ? levels[4] : Guid.Empty,
								UID = SequentialGuid.NewDbGuid(),
							};
							var resp2 = SaveConfigurationByUID(new SaveRequest<Configuration>(c));
							ErrorHandler.Check(resp2);
							result++;
						}
					}
				}
				return new SimpleResponse<int>(result);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<int>>(ex);
			}
		}


		public GetListResponse<Configuration> GetHierarchy(ConfigRequest req)
		{
			try
			{
				string name = Util.GetConnName<Configuration>();
				var db = ImardaDatabase.CreateDatabase(name);

				object[] args = new object[7];
				args[0] = req.ID;
				args[1] = 0; // hierarchy
				Guid[] levels = req.GetLevels();
				int n = levels.Length;
				for (int i = 0; i < n; i++) args[i + 2] = levels[i];

				using (IDataReader dr = db.ExecuteDataReader("GetConfiguration", args))
				{
					var list = new List<Configuration>();
					while (dr.Read())
					{
						var c = GetFromData<Configuration>(dr);
						list.Add(c);
					}
					list.Reverse(); // -> [0] becomes root
					return new GetListResponse<Configuration>(list);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Configuration>>(ex);
			}
		}

		public GetListResponse<Configuration> GetChildren(ConfigRequest req)
		{
			try
			{
				string name = Util.GetConnName<Configuration>();
				var db = ImardaDatabase.CreateDatabase(name);

				object[] args = new object[5];
				args[0] = req.ID;
				Guid[] levels = req.GetLevels();
				int n = Math.Min(levels.Length, 4);
				Array.Copy(levels, 0, args, 1, n);

				using (IDataReader dr = db.ExecuteDataReader("GetChildren", args))
				{
					var list = new List<Configuration>();
					while (dr.Read())
					{
						var c = GetFromData<Configuration>(dr);
						list.Add(c);
					}

					return new GetListResponse<Configuration>(list);
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Configuration>>(ex);
			}
		}

		public SimpleResponse<int> GetChildrenCount(ConfigRequest req)
		{
			try
			{
				string name = Util.GetConnName<Configuration>();
				var db = ImardaDatabase.CreateDatabase(name);

				object[] args = new object[5];
				args[0] = req.ID;
				Guid[] levels = req.GetLevels();
				int n = Math.Min(levels.Length, 4);
				Array.Copy(levels, 0, args, 1, n);

				int count = (int)db.ExecuteScalar("GetChildrenCount", args);
				return new SimpleResponse<int>(count);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<int>>(ex);
			}
		}
	}
}
