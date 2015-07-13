using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using System.Linq;

namespace FernBusinessBase
{
	/// <summary>
	/// Gets an instance of the given type, filled with data based on parameters.
	/// </summary>
	[MessageContract]
    public class TypeRequest : ParameterMessageBase
	{
		[MessageBodyMember]
		public string _TypeName;

		public Type EntityType
		{
			get
			{
				return Type.GetType(_TypeName);
			}
			private set
			{
				_TypeName = value.AssemblyQualifiedName;
			}
		}

		public TypeRequest()
		{
		}

		public TypeRequest(Type type)
			: this(type, null)
		{
		}

		public TypeRequest(Type type, Guid[] levels)
		{
			EntityType = type;
			Levels = levels;
		}

		public Guid[] Levels
		{
			get
			{
				if (HasParameters && Parameters.ContainsKey("Levels"))
				{
					string levels = Parameters["Levels"];
					if (string.IsNullOrEmpty(levels)) return new Guid[0];
					return levels.Split('|').Select(s => new Guid(s)).ToArray();
				}
				else return new Guid[0];
			}
			set
			{
				if (value != null || value.Length != 0)
				{
					Parameters["Levels"] = string.Join("|", value.Select(g => g.ToString()).ToArray());
				}
			}
		}
	}
}