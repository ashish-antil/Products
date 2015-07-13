/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;
using Imarda.Logging;

namespace ImardaCRMBusiness
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaCRM : FernBusinessBase.BusinessBase, IImardaCRM 
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "CRM Business";
		private ErrorLogger _Log = ErrorLogger.GetLogger("CRM");

	    public ImardaCRM()
	    {
            var reqAttributesUnderService = new IDRequest(Guid.Empty, "BusinessServiceName", "Imarda360.CRM");
            var response = LoadCachedAttributeListForBusinessService<AttributeValue>(reqAttributesUnderService);
	    }

		public override SimpleResponse<string> GetAttributes(IDRequest req)
		{
			return base.GetAttributes<Company>(req.ID); // can use any <T> that gets the right database connection! 
		}

        public BusinessMessageResponse GenericSaveEntity<T>(Guid companyID, EntityAttributes ea, object[] properties)
            where T : FullBusinessEntity, new()
        {
            var adList = new List<AttributeDefinition>();
            var avList = new List<AttributeValue>();

            var response = GenericSaveEntity<T>(companyID, ea, out adList, out avList, properties);
            if (adList != null && adList.Count > 0)
            {
                var attributingService = ImardaProxyManager.Instance.IImardaAttributingProxy;
                ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                {
                    channel = attributingService as IClientChannel;
                    var request = new SaveListRequest<FernBusinessBase.AttributeDefinition>(adList);
                    attributingService.SaveAttributeDefinitionList(request);
                });
            }

            if (avList != null && avList.Count > 0)
            {
                var attributingService = ImardaProxyManager.Instance.IImardaAttributingProxy;
                ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                {
                    channel = attributingService as IClientChannel;
                    var request = new SaveListRequest<FernBusinessBase.AttributeValue>(avList);
                    attributingService.SaveAttributeValueList(request);
                });
            }
            return response;
        }
	}
}
