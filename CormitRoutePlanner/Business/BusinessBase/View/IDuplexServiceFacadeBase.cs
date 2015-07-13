#region

using System.ServiceModel;

#endregion

namespace FernBusinessBase
{
    [ServiceContract(CallbackContract = typeof (ICallback))]
    public interface IDuplexServiceFacadeBase : IServerFacadeBase
    {
    }

    public interface ICallback
    {
    }
}