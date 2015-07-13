#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	/// <summary>
	///     A class common to all entity objects: business, application and smart client. -MV
	/// </summary>
	[DataContract]
	public abstract class BaseEntity
	{
		// ReSharper disable once InconsistentNaming
		private static readonly DateTime OLD_DATE_TIME = new DateTime(1753, 1, 3, 0, 0, 0);

		/// <summary>
		///     Prepare the object for sending it to another service, e.g. to copy
		///     non-DataMember properties to DataMember fields, or to compress or encrypt data.
		/// </summary>
		public virtual void PrepareForSave()
		{
		}

		[Conditional("DEBUG")]
		public static void ValidateThrow(FullBusinessEntity entity)
		{
			if (entity.ID == Guid.Empty)
			{
				throw new ValidationException(new[] { "ID is empty Guid: " + entity.GetType().Name });
			}

			string[] errors = entity.Validate(false);
			if (errors != null)
			{
				throw new ValidationException(errors);
			}
		}

		/// <summary>
		///     Validate the property values that have a constraint attribute.
		///     A property can have at most one constraint attribute, and it has to match its type.
		/// </summary>
		/// <param name="obj">object to validate</param>
		/// <param name="all">
		///     true to validate all properties with constraints, or false to stop after the first error
		/// </param>
		/// <returns>the list of errors or null if all valid.</returns>
		public static string[] Validate(object obj, bool all)
		{
			var errors = new List<string>();

			foreach (PropertyInfo pi in obj.GetType().GetProperties())
			{
				var attributes = pi.GetCustomAttributes(false);
				Attribute attr = null;
				foreach (object t in attributes)
				{
					bool valid = true;
					attr = (Attribute)t;
					Type type = attr.GetType();
					if (type == typeof(ValidLengthAttribute))
					{
						valid = ((ValidLengthAttribute)attr).IsValid((string)pi.GetValue(obj, null));
					}
					else if (type == typeof(ValidASCIIAttribute))
					{
						valid = ((ValidASCIIAttribute)attr).IsASCII((string)pi.GetValue(obj, null));
					}
					else if (type == typeof(ValidPatternAttribute))
					{
						valid = ((ValidPatternAttribute)attr).IsMatch((string)pi.GetValue(obj, null));
					}
					else if (type == typeof(ValidNonEmptyAttribute))
					{
						valid = ((ValidNonEmptyAttribute)attr).IsNonEmpty((Guid)pi.GetValue(obj, null));
					}
					else if (type == typeof(ValidIntAttribute))
					{
						valid = ((ValidIntAttribute)attr).InRange((int)pi.GetValue(obj, null));
					}
					else if (type == typeof(InvalidPatternAttribute))
					{
						valid = ((InvalidPatternAttribute)attr).IsMatch((string)pi.GetValue(obj, null));
					}
					if (!valid)
					{
						string msg = pi.Name + " violates " + attr;
						if (all) errors.Add(msg);
						else return new[] { msg };
					}
				}
			}
			return errors.Count > 0 ? errors.ToArray() : null;
		}

		public virtual void AssignData(IDataReader dr)
		{
		}

		public static bool HasColumn(IDataReader dr, string field)
		{
			return ((ImardaDataReader)dr).IndexOf(field) >= 0;
		}

		public static DateTime GetDateTime(IDataReader dr, string field)
		{
			var dt = GetValue<DateTime>(dr, field);
			if (dt <= OLD_DATE_TIME) return DateTime.MinValue;
			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		public static DateTime? GetNullableDateTime(IDataReader dr, string field)
		{
			DateTime? dt = GetNullable<DateTime>(dr, field);
			if (!dt.HasValue || dt.Value <= OLD_DATE_TIME)
			{
				return null;
			}

			return DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
		}

		public static char GetChar(IDataReader dr, string field)
		{
			var value = dr[field] as string;
			return string.IsNullOrEmpty(value) ? '\0' : value[0];
		}

		public static char? GetNullableChar(IDataReader dr, string field)
		{
			var value = dr[field] as string;
			return string.IsNullOrEmpty(value) ? (char?)null : value[0];
		}

		public static T GetValue<T>(IDataReader dr, string field)
		{
			object value = dr[field];
			if (value is DBNull) return default(T);
			if (typeof(T) == typeof(char)) throw new ArgumentException("Replace GetValue<char> by GetChar: " + field);
			if (value is T) return (T)value;
			return default(T);
		}

		public static T? GetNullable<T>(IDataReader dr, string field)
			where T : struct
		{
			object value = dr[field];
			if (value is DBNull) return null;
			if (typeof(T) == typeof(char)) throw new ArgumentException("Replace GeNullable<char> by GetNullableChar: " + field);
			if (value is T) return (T)value;
			return null;
		}
	}

	[DataContract]
	public class BusinessEntity : BaseEntity
	{
		public BusinessEntity()
		{
			DateModified = DateCreated = DateTime.UtcNow;
			Active = true;
			Deleted = false;
		}

		[DataMember]
		public DateTime DateCreated { get; set; }

		[DataMember]
		public DateTime DateModified { get; set; }

		[DataMember]
		public bool Deleted { get; set; }

		[DataMember]
		public bool Active { get; set; }

		public void DeleteAndDeactivate()
		{
			Active = false;
			Deleted = true;
		}

		public static void Copy(BusinessEntity e1, BusinessEntity e2)
		{
			e2.DateCreated = e1.DateCreated;
			e2.DateModified = e1.DateModified;
			e2.Deleted = e1.Deleted;
			e2.Active = e1.Active;
		}

		/// <summary>
		///     Assigns the properties of this BusinessEntity based on the given DataRow.
		/// </summary>
		/// <param name="dr">The DataRow with which this object's properties are to be obtained</param>
		public override void AssignData(IDataReader dr)
		{
			Active = GetValue<bool>(dr, "Active");
			Deleted = GetValue<bool>(dr, "Deleted");
			DateModified = GetDateTime(dr, "DateModified");
			DateCreated = GetDateTime(dr, "DateCreated");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

		public override string ToString()
		{
			return string.Format("{0}({1:s})", GetType().Name, DateModified);
		}
	}
}