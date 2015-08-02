

using System.ServiceModel;
using FernBusinessBase;

using Imarda360Application.CRM;





// ReSharper disable once CheckNamespace
namespace Cormit.Application.RouteApplication
{
    [ServiceContract]
    public interface ICormit24 : IImardaCRM,
        

        IImardaSolution
    {
        [OperationContract]
        SimpleResponse<string[]> GetMessage(IDRequest request);
    }
}