using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using FernBusinessBase.Extensions;
using Imarda.Lib;
//using ImardaAttributingBusiness;
//using ImardaAttributingBusiness;

namespace FernBusinessBase
{
	/// <summary>
	/// A BusinessEntity that also includes ID, FernID, and FernUser.
	/// Any non - map classes should inherit from this, rather than BusinessEntity, to make life easier.
	/// </summary>
	[DataContract]
	public class FullBusinessEntity : BusinessEntity
	{
		public const string SystemGuidStr = "11111111-1111-1111-1111-111111111111";
		public const string ErrorGuidStr = "44444444-4444-4444-4444-444444444444";
		public static readonly Guid SystemGuid = new Guid(SystemGuidStr);
		public static readonly Guid ErrorGuid = new Guid(ErrorGuidStr);

        //public Proxy<IImardaAttributing> IImardaAttributing = new Proxy<IImardaAttributing>("AttributingTcpEndpoint");
        
        [DataMember]
		public Guid UserID { get; set; }

		[DataMember]
		public Guid CompanyID { get; set; }

		[BasicSpIgnore]
		[DataMember]
		public string Path { get; set; }			//& gs-351

		[DataMember]
		public Guid ID { get; set; }
		
		[BasicSpIgnore]
		[DataMember]
		public bool IsTemplate { get; set; }		//& gs-352

		public static void Copy(FullBusinessEntity e1, FullBusinessEntity e2, bool copyID)
		{
			Copy(e1, e2);
			if (copyID)
				e2.ID = e1.ID;
			e2.UserID = e1.UserID;
			e2.CompanyID = e1.CompanyID;
			e2.Path = e1.Path;
			e2.IsTemplate = e1.IsTemplate;
		    e2.AttributeValues = e1.AttributeValues;                    //&IM-3747
		    e2.AttributeValuesModified = e1.AttributeValuesModified;    //&IM-3747
		}

		protected Guid EnsureSequential(Guid id)
		{
			if (SequentialGuid.IsSequential(id)) return id;
			throw new ArgumentException(GetType().Name + ".ID must be Sequential Guid - " + id);
		}

		[BasicSpIgnore]
		[DataMember]
		public EntityAttributes Attributes { get; set; }

        //[BasicSpIgnore]
        //public List<FernBusinessBase.AttributeValue> BaseAttributeValues { get; set; }   //  IM-3747

        [DataMember]
        public List<FernBusinessBase.AttributeValue> AttributeValues { get; set; }

        [DataMember]
        public List<FernBusinessBase.AttributeValue> AttributeValuesModified { get; set; }

        [DataMember]
        public bool Added = false;
        
        [DataMember]
        public bool Changed = false;

        public T GetAttributeValue<T>(string attributeName) //IM-3747
        {
            string lowerAttributeName = attributeName.ToLower();
            //var value = AttributeValues.FirstOrDefault(s => s.VarName == attributeName).GetValue<T>();
            var value = AttributeValues.FirstOrDefault(s => (s.VarName.ToLower() == lowerAttributeName || s.FriendlyName.ToLower() == lowerAttributeName));
            if (value == null)
                return default(T);
            return value.GetValue<T>();
        }

        /// <summary>
        /// This will return the AttributeValue entity of a specific attribute in your AttributeValue collection e.g. in list of unit attributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityAttributes"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        //public FernBusinessBase.AttributeValue GetAttributeValue(string attributeName) //WASDJ
        public FernBusinessBase.AttributeValue GetAttributeValue(string attributeName)
        {
            if (AttributeValues == null) return null;
            string lowerAttributeName = attributeName.ToLower();
            return AttributeValues.FirstOrDefault(s => (s.VarName.ToLower() == lowerAttributeName || s.FriendlyName.ToLower() == lowerAttributeName));
        }

        public FernBusinessBase.AttributeValue GetAttributeValueModified(string attributeName)
        {
            if (AttributeValuesModified == null) return null;
            string lowerAttributeName = attributeName.ToLower();
            return AttributeValuesModified.FirstOrDefault(s => (s.VarName.ToLower() == lowerAttributeName || s.FriendlyName.ToLower() == lowerAttributeName));
        }

        /// <summary>
        /// Use this is you want to change multiple attributes for an entity, such as Unit,
        /// and then later save all changes to cache and disk in one go, when saving the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        public void SetAttributeValue<T>(string varName, T value)
        {
            FernBusinessBase.AttributeValue attributeValue = GetAttributeValue(varName);

            if (attributeValue == null)     // Create attribute on-the-fly if not found
            {
                attributeValue = CreateAttributeValue(varName, value);
                AttributeValues.Add(attributeValue);
                attributeValue.Added = true;        // flag for saving this new attribute to cache and disk
                if (AttributeValuesModified == null)
                    AttributeValuesModified = new List<FernBusinessBase.AttributeValue>();
                AttributeValuesModified.Add(attributeValue);    // mark as added
            }
            else
            {
                attributeValue.Changed = true;      // flag for saving changes to this attribute to cache and disk
                // Move curent attribute value to previous
                attributeValue.PrevValue = attributeValue.Value;
                attributeValue.PrevDateModified = attributeValue.DateModified;
                attributeValue.DateModified = DateTime.UtcNow;
                if (AttributeValuesModified == null)
                    AttributeValuesModified = new List<FernBusinessBase.AttributeValue>();
                AttributeValuesModified.Add(attributeValue);    // mark as changed
            }

            if (value is IMeasurement)
            {
                var m = (IMeasurement)value;
                attributeValue.Value = Measurement.VString(m);
            }
            else
                attributeValue.Value = value.ToString();
            FlagAttributeValueAsModified(attributeValue);
        }
        public void FlagAttributeValueAsModified(FernBusinessBase.AttributeValue attributeValue)
	    {
	        var attributeValueModified = GetAttributeValueModified(attributeValue.VarName);
            if (attributeValueModified == null)     // attribute not already in modified list, so add it
            {
                if (AttributeValuesModified == null)
                    AttributeValuesModified = new List<FernBusinessBase.AttributeValue>();
                AttributeValuesModified.Add(attributeValue);
            }
            else
            {
                attributeValueModified.Value = attributeValue.Value;
                attributeValueModified.DateModified = DateTime.UtcNow;
            }
        }

	    /// <summary>
	    /// Used to create a new AttributeValue on-the-fly (this implies a new AttributeDefinition will be needed too)
	    /// </summary>
	    /// <param name="varName"></param>
	    /// <param name="value"></param>
	    /// <returns></returns>
        private FernBusinessBase.AttributeValue CreateAttributeValue<T>(string varName, T value)
	    {
	        string varType = "$";   // default to string type
	        string format = "";     // default format
	        if (value is IMeasurement)
	        {
                varType = "!" + value.GetType().Name;
	            format = ((IMeasurement)value).MetricSymbol;
	        }

            return new FernBusinessBase.AttributeValue
	        {
	            ID = Guid.NewGuid(),
	            CompanyID = CompanyID,
	            UserID = UserID,
	            DateCreated = DateTime.UtcNow,
	            DateModified = DateTime.UtcNow,
	            Active = true,
	            Deleted = false,
	            VarName = varName,
	            FriendlyName = varName,
	            Description = varName,
                GroupID = Guid.Empty,
                VarType = varType,
                Format = format,
	            CaptureHistory = false,
	            Viewable = true,
                EntityTypeName = this.GetType().Name,
                AttributeID = Guid.NewGuid(),
	            EntityID = this.ID
	        };
	    }

        //public void SaveAttributeValue<T>(string varName, T attribValue)
        //{
        //    var proxy = Proxies.Instance.IImardaAttributingProxy;
        //    var request = new IDRequest(entity.ID, "EntityTypeName", entityTypeName);
        //    var response = proxy.Invoke(service => service.GetAttributeValueListByEntityType_EntityID(request));
        //    if (response.List == null || response.List.Count == 0)
        //    {
        //        var s = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        Gateway.Log.WarnFormat("AtomLibrary.cs - " + s + " - returns null for ID: {0}", entity.ID);
        //    }
        //}

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ID = GetValue<Guid>(dr, "ID");
			CompanyID = GetValue<Guid>(dr, "CompanyID");
			UserID = GetValue<Guid>(dr, "UserID");
			if (HasColumn(dr, "Path"))					//& gs-351
				Path = GetValue<string>(dr, "Path");
			if (HasColumn(dr, "IsTemplate"))		    //& gs-352
				IsTemplate = GetValue<bool>(dr, "IsTemplate");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif

		}

		/// <summary>
		/// Make sure changes to the Attributes.Map gets saved back to the string representation
		/// before saving the item.
		/// </summary>
		public override void PrepareForSave()
		{
			base.PrepareForSave();
			if (Attributes != null) Attributes.UpdateAttributeString();
		}


		/// <summary>
		/// Validate this object's properties. This method can be overridden in the subclasses
		/// to do any specific validation. The virtual method performs the attribute-based 
		/// validation only.
		/// </summary>
		/// <param name="all">true to validate all properties with constraints and return 
		/// complete list of errors</param>
		/// <returns>null if OK, or string[] if errors; each item in the array has a property name</returns>
		public virtual string[] Validate(bool all)
		{
			return Validate(this, all);
		}

		public override string ToString()
		{
			return string.Format("{0}({1},{2:s}{3})",
				GetType().Name, ID, DateModified,
				Attributes == null ? "" : (Attributes.HasAttributes ? ",+EA" : ""));
		}

		/// <summary>
		/// Gets an array containing the properties which are always included when saving to DB, etc
		/// </summary>
		/// <returns></returns>
		public object[] GetBaseProperties()
		{
			var properties = new object[]
			             {
				             ID,
				             Active,
				             Deleted,
				             DateCreated,
				             DateModified = DateTime.UtcNow,
				             CompanyID,
				             UserID
			             };
			return properties;
		}

		/// <summary>
		/// Returns a new array in which the base properties are inserted
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		public object[] InsertBaseProperties(object[] properties)
		{
			var baseProps = GetBaseProperties();
			properties = baseProps.Concat(properties).ToArray();
			return properties;
		}

        //public void SaveAttributeValue<T>(IImardaAttributing proxy, string varName, T attribValue)
        //{
        //    FernBusinessBase.AttributeValue attributeValue = this.GetAttributeValue(varName);
        //    attributeValue.Value = attribValue.ToString();

        //    var request = new SaveRequest<FernBusinessBase.AttributeValue>(attributeValue);
        //    var responseSaveAttribute = proxy.Invoke<IImardaAttributing>(service => service.GetAttributes().SaveAttributeValue(request));
        //}

	}

	public class BasicSpIgnore : Attribute
	{
	}

}
