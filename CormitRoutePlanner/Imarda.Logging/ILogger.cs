namespace Imarda.Logging
{
    public interface ILogger
    {
        void Verbose(object e);
        void Debug(object e);
        void Info(object e);
        void Warn(object e);
        void Error(object e);
        void Sys(object e);
        void VerboseFormat(string fmt, params object[] e);
        void DebugFormat(string fmt, params object[] e);
        void InfoFormat(string fmt, params object[] e);
        void WarnFormat(string fmt, params object[] e);
        void ErrorFormat(string fmt, params object[] e);
        void SysFormat(string fmt, params object[] e);

	    string Name { get; }
		int MaxLevel { get; set; }
    }
}