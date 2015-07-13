

using System.ServiceModel;
using FernBusinessBase;





// ReSharper disable once CheckNamespace
namespace Imarda360Application
{
    [ServiceContract]
    public interface IImarda360 : IImardaSolution
    {
        [OperationContract]
        SimpleResponse<string[]> GetMessage(IDRequest request);
    }
}