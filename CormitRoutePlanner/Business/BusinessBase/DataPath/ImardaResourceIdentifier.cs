using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace FernBusinessBase
{
	/// <summary>
	/// IRI, Imarda Resource Identifier. Something like URI, but with our own rules.
	/// </summary>
	[DataContract]
	public struct Iri
	{
		public string _Path;

		[DataMember]
		public string Path
		{
			get { return _Path; }
			set { _Path = value; }
		}

		public static Iri LiteralTyped(params object[] args)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < args.Length; i += 2)
			{
				var key = ((string)args[i]).ToLowerInvariant();
				sb.AppendKV(key, args[i + 1]);
			}
			return new Iri("tlit://" + sb.ToString());
		}

		public static Iri Literal(string args) // use params string[] instead.
		{
			return new Iri("lit://" + args);
		}

		public static Iri StoredProc(string db, string sp, params object[] args)
		{
			return new Iri(string.Format("sp://{0}/{1}/{2}", db, sp, args[0])); //TODO interpretet args[1]..args[n] as well. Use CSV format.
		}

		public static Iri Record(string db, string table, string key)
		{
			return new Iri(string.Format("rec://{0}/{1}/{2}", db, table, key));
		}


		public Iri(string path)
		{
			if (path != null && !path.Contains("://")) throw new ArgumentException(string.Format("Invalid path: [{0}]", path));
			_Path = path;
		}

		public T GetEntity<T>(object parent) where T : BusinessEntity, new()
		{
			// rec://Tracking/OtrLoadInfo/ID

			int i = Path.IndexOf("://");
			if (i >= 0)
			{
				string scheme = Path.Substring(0, i);
				string query = Path.Substring(i + 3);
				string[] parts = query.Split('/');
				T entity = new T();
				string sp;
				if (Path.StartsWith("rec")) sp = "SPGet" + parts[1];
				else if (Path.StartsWith("sp")) sp = parts[1];
				else throw new InvalidOperationException("Iri.GetEntity scheme not supported in " + Path);

				return GetEntityBySP(parent, parts[0], sp, parts[2], entity) ? entity : null;
			}
			throw new InvalidOperationException("Iri.GetEntity no '://' found in " + Path);
		}

        //IM-3747 - never used
        //public EntityAttributes GetData(object parent)
        //{
        //    // Valid paths:
        //    // sp://Tracking/SPGetOTRLoadInfo/ID  -> Execute SP to get entity record
        //    // attr://Tracking/ID  -> Load from EntityAttributes table
        //    // lit://k1|v1||k2|v2  -> literal attribute string contained in datapath (but there are length restrictions)

        //    int i = Path.IndexOf("://");
        //    string retrievalMethod = Path.Remove(i);
        //    string query = Path.Substring(i + 3);
        //    string[] parts = query.Split('/');
        //    EntityAttributes ea = null;
        //    if (i == -1)
        //    {
        //        ea = new EntityAttributes("");
        //    }
        //    else
        //    {
        //        switch (retrievalMethod)
        //        {
        //            case "sp":
        //                ea = GetEntityAttributesBySP(parent, parts[0], parts[1], parts[2]);
        //                break;
        //            case "attr":
        //                ea = GetFromEntityAttributesTable(parent, parts[0], parts[1]);
        //                break;
        //            case "lit":
        //                ea = new EntityAttributes(query);
        //                break;
        //        }
        //    }
        //    return ea;
        //}


		public IDictionary GetKeyValueMap()
		{
			// tlit://k1t1|v1||k2t2|v2 -> typed valus, e.g. "odo!Length ~dist|1234000||name$|John"
			const string tlit = "tlit://";
			if (string.IsNullOrEmpty(Path)) return new Dictionary<string, object>();
			if (!Path.StartsWith(tlit)) throw new Exception("Require tlit://");
			string kv = Path.Substring(tlit.Length);
			var ht = new Hashtable();
			var dict = new Dictionary<string, object>();
			EAHelper.Deserialize(Path, dict);
			return dict;
		}

		private static object GetPropertyFromEntity(object parent, string name)
		{
			// First check for literals:
			char c = name[0];
			string literal = name.Substring(1);
			if (c == '$') return literal;
			if (c == '*') return new Guid(literal);
			if (c == '!')
			{
				int i;
				if (int.TryParse(literal, out i)) return i;
				else return null;
			}

			// Interpret name as the name of a property in the parent entity.
			var propertyInfo = parent.GetType().GetProperty(name);
			return (propertyInfo != null) ? propertyInfo.GetValue(parent, null) : null;
		}

        //IM-3747 - never used
        //private static EntityAttributes GetFromEntityAttributesTable(object entity, string dbname, string propertyName)
        //{
        //    // attr://Tracking/ID  -> Load from EntityAttributes table
        //    string name = "Imarda" + dbname + "BusinessInterface"; // e.g. "ImardaTrackingBusinessInterface", other names e.g.: "GIS", "VehicleManagement", "Metrics"
        //    var db = ImardaDatabase.CreateDatabase(name);
        //    object arg = GetPropertyFromEntity(entity, propertyName);
        //    var kvstring = db.ExecuteScalar("SPGetEntityAttributes", arg) as string;
        //    return new EntityAttributes(kvstring);
        //}

		/// <summary>
		/// Get the entity attributes by calling a SP that returns an entity. The entity is turned into an EntityAttribute string.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="parts">[0]=part of db connection string, [1]=stored proc name, [2] = propertyName</param>
		/// <returns></returns>
        // IM-3747 - never used
        //private static EntityAttributes GetEntityAttributesBySP(object parent, string dbname, string storedProc, string propertyName)
        //{
        //    // sp://Tracking/SPGetOTRLoadInfo/ID  -> Execute SP to get entity record
        //    string name = "Imarda" + dbname + "BusinessInterface"; // e.g. "ImardaTrackingBusinessInterface", other names e.g.: "GIS", "VehicleManagement", "Metrics"
        //    var db = ImardaDatabase.CreateDatabase(name);
        //    object arg = GetPropertyFromEntity(parent, propertyName);
        //    var kvstring = GetKeyValueStringFromDataRow(db, storedProc, arg);
        //    return new EntityAttributes(kvstring);
        //}

		/// <summary>
		/// Load an entity from the database. 
		/// </summary>
		/// <param name="parent">parent object containing the parameters for the query</param>
		/// <param name="dbname">part of the connection string of the database that contains the SP</param>
		/// <param name="storedProc">called SP, returns entity data</param>
		/// <param name="propertyName"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		private static bool GetEntityBySP(object parent, string dbname, string storedProc, string propertyName, BusinessEntity entity)
		{
			//         [0]         [1]         [2]
			// sp://Tracking/SPGetOTRLoadInfo/ID  -> Execute SP to get entity record
			string name = "Imarda" + dbname + "BusinessInterface"; // e.g. "ImardaTrackingBusinessInterface", other names e.g.: "GIS", "VehicleManagement", "Metrics"
			var db = ImardaDatabase.CreateDatabase(name);
			object arg = GetPropertyFromEntity(parent, propertyName);
			using (IDataReader dr = db.ExecuteDataReader(storedProc, arg))
			{
				if (dr.Read())
				{
					entity.AssignData(dr);
					return true;
				}
			}
			return false;
		}

		private static string GetKeyValueStringFromDataRow(ImardaDatabase db, string sp, object arg)
		{
			var sb = new StringBuilder();

			using (IDataReader dr = db.ExecuteDataReader(sp, arg))
			{
				if (dr.Read())
				{
					for (int i = 0; i < dr.FieldCount; i++)
					{
						string key = dr.GetName(i);
						sb.AppendKV(key, dr[i]);
					}
				}
			}
			return sb.ToString();
		}

	}
}
