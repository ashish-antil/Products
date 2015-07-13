using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	partial interface IImardaCRM
	{
		[OperationContract]
		BusinessMessageResponse DeleteContactMapByContactNPerson(IDRequest request); 
	}
}
