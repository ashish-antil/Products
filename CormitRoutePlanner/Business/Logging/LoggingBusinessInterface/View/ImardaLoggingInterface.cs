using System.ServiceModel;

namespace ImardaLoggingBusiness 
{
	[ServiceContract]
    public interface IImardaLogging
	{
        [OperationContract]
        bool SaveLogToDatabase(Logging request);
	}
}

