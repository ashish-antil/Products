#region

using System.ServiceModel;
using FernBusinessBase;
//using FernBusinessBase.Interfaces;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaAttributingBusiness 
{
	[ServiceContract]
    public partial interface IImardaAttributing : IServerFacadeBase //, IAccessAttributingService 
	{
	}
}

