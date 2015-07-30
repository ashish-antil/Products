using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaCRMBusiness;

namespace Imarda360Application.CRM
{
	partial interface IImardaCRM
	{
		[OperationContract]
		BusinessMessageResponse SaveContactMap(SaveRequest<ContactMap> request);
   		[OperationContract]
		BusinessMessageResponse DeleteContactMapByContactNPerson(IDRequest request);
}
}