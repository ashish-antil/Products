using System.Diagnostics;
using System.Runtime.Serialization;

namespace ImardaLoggingBusiness
{
    public enum Priority
    {
        Low = 3,
        Normal = 2,
        High = 1
    }

    public struct Category
    {
        public const string General = "General";
        public const string Debug = "Debug";
        public const string Info = "Info";
        public const string Warn = "Warn";
        public const string Error = "Error";
        public const string Verbose = "Verbose";
    }

    [DataContract]
    public class Logging 
    {
        public Logging()
        {
        }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public TraceEventType Severity { get; set; }

        [DataMember]
        public int LogLevel { get; set; }
    }
}

