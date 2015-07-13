using Imarda.Lib.Utilities;

namespace FernBusinessBase.Model.Extensions
{
	public static class EntityExtensions
	{
		/// <summary>
		/// Gets an array of property values in alpha order for the entity to save in DB
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="insertBaseProperties">Insert in first position the properties of FullBusinessEntity</param>
		/// <returns></returns>
		public static object[] GetSpSaveProperties<T>(this T me, bool insertBaseProperties= true)
			where T: FullBusinessEntity
		{
			var objs = ReflectionUtils.GetPropertyValues(me, true,true,true);
			if (!insertBaseProperties)
			{
				return objs;
			}
			return me.InsertBaseProperties(objs);
		}

		/// <summary>
		/// Gets an array of property values in alpha order for the entity to save in DB including any base class properties
		/// in this case the properties will be in alpha order including the base class properties unless they have the BasicSpIgnore attribute
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <returns></returns>
		public static object[] GetSpSavePropertiesWithBase<T>(this T me)
			where T : FullBusinessEntity
		{
			//get all unsorted properties for this guy including whatever base class(es) can be on the way up to FullBusinessEntity
			var objs = ReflectionUtils.GetPropertyValues(me, true, false, true,typeof(BasicSpIgnore));
			return objs;
		}

	}
}
