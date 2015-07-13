using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Logging;

using Imarda360Application.Task;

using Imarda.Lib;
using System.Text;


namespace Imarda360Application.Task
{
	internal static class ActionHelper
	{
		private static Dictionary<string, Guid> _EntityMap;
		private static ErrorLogger _Log = ErrorLogger.GetLogger("ActionHelper");

		static ActionHelper()
		{
			var attributeTypes = new Dictionary<string, Type>
			{
				{ "veh.fleets", typeof(string[]) },
				{ "veh.driver", typeof(string)},
				{ "dev.locations", typeof(string[]) },
			};
			var entities = new[] { "veh", "drv", "dev", "loc" };
			
			_EntityMap = new Dictionary<string, Guid>
			{
				{ "veh", new Guid("e9f4f44c-3a6a-4002-87a0-6e7f4f38ff69") },
				{ "drv", new Guid("a5e2bf3c-110c-4a4c-b72e-9def30ff0f6d") },
				{ "dev", new Guid("26b8d08c-2764-4a66-b5b2-8f31dc75acbd") },
				{ "loc", new Guid("5292523a-43d4-4c90-a6be-ca7f70e80fe0") },
			};
		}

		/// <summary>
		/// Get the event category id for the entity type mnemonic
		/// </summary>
		/// <param name="entityTypeMnemonic">"veh", "drv", etc.</param>
		/// <returns>Guid.Empty if not found or category guid</returns>
		internal static Guid GetEventCategoryForEntity(string entityTypeMnemonic)
		{
			Guid categoryID;
			return _EntityMap.TryGetValue(entityTypeMnemonic, out categoryID) ? categoryID : Guid.Empty;
		}
	}
}
