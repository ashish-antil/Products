
#region

using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;
using System;
//using FernBusinessBase.Interfaces;
using Imarda.Logging;

#endregion

namespace ImardaAttributingBusiness
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaAttributing : BusinessBase, IImardaAttributing
	{
	    public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
	    public const string Description = "Attributing Business";

	    private readonly ErrorLogger _Log = ErrorLogger.GetLogger("Attributing");
		//private readonly BusinessObjectCache<UnitPosition> _UnitPositionCache = BusinessObjectCache<UnitPosition>.Instance;
		//private DateTime? _ResumeCaching;


	    private Dictionary<string, Guid> _lookup = new Dictionary<string, Guid>();  //TEMPDJ - undecided if needed


	    //public ImardaAttributing()
	    //{
	    //    try
	    //    {
	    //        //_Log.Info("ImardaAttributing Constructor - begin");



	    //        //_Log.Info("ImardaAttributing Constructor - end");
	    //    }
	    //    catch (Exception ex)
	    //    {
	    //        _Log.ErrorFormat("ImardaAttributing Constructor Exception {0}", ex);
	    //    }
	    //}

	    //public override SimpleResponse<string> GetAttributes(IDRequest req)
	    //{
	    //    return GetAttributes<Unit>(req.ID); // can use any <T> that gets the right database connection! 
	    //}

        //void IAccessAttributingService.SaveAttributeValue(SaveRequest<FernBusinessBase.AttributeValue> request)
        //{
        //    throw new NotImplementedException();
        //}

        //void IAccessAttributingService.SaveAttributeDefinition(SaveRequest<FernBusinessBase.AttributeDefinition> request)
        //{
        //    throw new NotImplementedException();
        //}
        //public BusinessMessageResponse SaveAttributeValueDave(SaveRequest<FernBusinessBase.AttributeValue> request)
        //{
        //    throw new NotImplementedException();
        //}

        //public BusinessMessageResponse SaveAttributeDefinitionDave(SaveRequest<FernBusinessBase.AttributeDefinition> request)
        //{
        //    throw new NotImplementedException();
        //}
	}
}

