

using System.ServiceModel;
using FernBusinessBase;

using Imarda360Application.CRM;
using IImardaSecurity = Cormit.Application.RouteApplication.Security.IImardaSecurity;




// ReSharper disable once CheckNamespace
namespace Cormit.Application.RouteApplication
{
    [ServiceContract]
    public interface ICormit24 : IImardaCRM,IImardaSecurity,IImardaSolution
    {
        [OperationContract]
        SimpleResponse<string[]> GetMessage(IDRequest request);
    }
}