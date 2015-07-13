using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cormit.Application.Tracking;
using CormitRouteTrackingApplicationLib.Tracking;
using FernBusinessBase;
using FernBusinessBase.Errors;
using FernBusinessBase.Extensions;
using FernBusinessBase.View.Generics;
using Imarda.Lib;

using Imarda360Application.Tracking;
using Imarda360Base;
using System.Globalization;
using ConfigUtils = Imarda.Lib.ConfigUtils;



//  _ _____  __    ___       _                  _____                   _      
// (_)___ / / /_  / _ \     / \   _ __  _ __   |  ___|_ _  ___ __ _  __| | ___ 
// | | |_ \| '_ \| | | |   / _ \ | '_ \| '_ \  | |_ / _` |/ __/ _` |/ _` |/ _ \
// | |___) | (_) | |_| |  / ___ \| |_) | |_) | |  _| (_| | (_| (_| | (_| |  __/
// |_|____/ \___/ \___/  /_/   \_\ .__/| .__/  |_|  \__,_|\_ _\__,_|\__,_|\___|
//                               |_|   |_|                 _|                  
//Imarda360.cs

// ReSharper disable once CheckNamespace
namespace Imarda360Application
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class Imarda360 : BusinessBase, IImarda360
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "i360 Application";

		//private static readonly ErrorLogger _Log = ErrorLogger.GetLogger("i360");


		private readonly MethodCallLogger _CallLogger;

        private readonly ICormitRouteTracking _Tracking;
		private readonly IImardaSolution _Solution;
		

		public Imarda360()
		{
			_Solution = FacadeImplementation.Instance;
            _Tracking = CormitRouteTracking.Instance;            
			//_CallLogger = new MethodCallLogger(_Log);

		}

		private void LogResult(IRequestBase request, object response)
		{
			_CallLogger.LogResult(request, response);
		}


		private void Log(string name, IRequestBase request)
		{
			_CallLogger.LogCall(name, request);
		}


		

		private static void ValidateRequest<T>(Save360Request<T> request)
			where T : SolutionEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}

		private static void ValidateRequest<T>(SaveRequest<T> request)
			where T : BaseEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}

		private static void ValidateRequest<T>(SaveListRequest<T> request)
			where T : BaseEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}


		private static void ValidateEntity(object entity)
		{
			string[] errors = BaseEntity.Validate(entity, true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}









        public SimpleResponse<string[]> GetMessage(IDRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
