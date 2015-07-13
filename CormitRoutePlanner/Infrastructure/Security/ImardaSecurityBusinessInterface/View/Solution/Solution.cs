using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity {
		[OperationContract]
		GetListResponse<Solution> GetSolutionList();

		[OperationContract]
		BusinessMessageResponse SaveSolutionList(SaveListRequest<Solution> request);

		[OperationContract]
		BusinessMessageResponse DeleteSolution(IDRequest request);
	}
}
