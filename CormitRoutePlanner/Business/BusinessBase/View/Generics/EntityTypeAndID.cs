#region

using System;
using System.Runtime.Serialization;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	/// <summary>
	/// Object contains entity type and entity ID. Meant as information to construct
	/// a call that retrieves the full entity information.
	/// </summary>
	[DataContract]
	[Serializable]
	public class EntityTypeAndID
	{
		public EntityTypeAndID(Type type, Guid id)
		{
			_TypeName = type.AssemblyQualifiedName;
			_ID = id;
		}

		[DataMember]
		public string _TypeName;

		[DataMember]
		public Guid _ID;


		public Type EntityType
		{
			get
			{
				return Type.GetType(_TypeName);
			}
			set
			{
				_TypeName = value.AssemblyQualifiedName;
			}
		}

		public Guid ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

	}
}