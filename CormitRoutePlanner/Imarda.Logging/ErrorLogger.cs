using System.Diagnostics;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Imarda.Lib;


namespace Imarda.Logging
{
    public enum NewFile
    {
        StartupOnly = 0,
        UtcMidnight = -1,
        /* positive numbers are megabytes max file size */
    }

    public class ErrorLogger : ILogger
    {
        private const long MB = 1048576L;
        private const string DefaultLogFolder = @"c:\i360.Logs\ErrorLogs";
        private static readonly TimeSpan _24h = TimeSpan.FromHours(24.0);

        private static readonly Dictionary<string, ErrorLogger> _Logs;

        private readonly object _FileAccess = new object();
        private readonly string _FileBaseName;
        private readonly string _Name;
        private readonly NewFile _NewFile;
        private readonly int _UdpPort;
        private readonly string _UdpLogServer;
        private FileStream _Fs;
        private DateTime _NewFileCreationTime;
        private string _Path;
        private readonly Tally _Tally;

        private readonly int _EventLevel = 1;
        private readonly int _TallyLevel = 1;
        private int _MaxLevel = 100;
        private bool _Initialized;
        private static string loggingServiceUrl = string.Empty;

        /// <summary>
		/// -1 = no logging to the database, 1 = logs to the file location
		/// </summary>
		private static int _LogToDatabase = -1;

        private Regex _RxInclude, _RxExclude;

        static ErrorLogger()
        {
            _Logs = new Dictionary<string, ErrorLogger>();
            LoadSettings();
        }

        private ErrorLogger(int port)
        {
            _UdpLogServer = "127.0.0.1";
            _UdpPort = port;
            LoadSettings();
        }

        /// <summary>
        /// Create an NewLogger. 
        /// </summary>
        /// <param name="dir">base folder</param>
        /// <param name="fileName">name of the log</param>
        /// <param name="suffix">something to append to the fileName to make it unique</param>
        /// <param name="newFileCriterion">defines when to close current file and create a new file</param>
        /// <param name="maxLevel">maximum error level that gets logged, higher values get ignored</param>
        /// <param name="eventLevel">messages with an log level less than or equal to this number will also be sent to the OS Event Log</param>
        /// <param name="tallyLevel">for levels between 1 and tallyLevel, only show text first time, after that show a checksum and a count</param>
        private ErrorLogger(string dir, string fileName, string suffix, NewFile newFileCriterion, int maxLevel, int eventLevel, int tallyLevel)
        {
            try
            {
                _EventLevel = eventLevel;
                _TallyLevel = tallyLevel;
                _MaxLevel = maxLevel;
                _NewFile = newFileCriterion;
                _Tally = new Tally();
                _Name = fileName + suffix;
                _FileBaseName = Path.Combine(dir, _Name);

                if (_LogToDatabase == 1)
                {
                    byte[] bytes = new byte[0];
                  //  bool response = SaveLogToDatabase(bytes, 0);

                  //  if (!response) CreateFiles(dir, fileName, suffix);
                }
                else
                {
                    CreateFiles(dir, fileName, suffix);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Create an NewLogger. 
        /// </summary>
        /// <param name="udpLogServer">Server to send log info via udp to</param>
        /// <param name="port">Port to send log info via udp to</param>
        /// <param name="dir">base folder</param>
        /// <param name="fileName">name of the log</param>
        /// <param name="suffix">something to append to the fileName to make it unique</param>
        /// <param name="newFileCriterion">defines when to close current file and create a new file</param>
        /// <param name="maxLevel">maximum error level that gets logged, higher values get ignored</param>
        /// <param name="eventLevel">messages with an log level less than or equal to this number will also be sent to the OS Event Log</param>
        /// <param name="tallyLevel">for levels between 1 and tallyLevel, only show text first time, after that show a checksum and a count</param>
        private ErrorLogger(string udpLogServer, int port, string dir, string fileName, string suffix, NewFile newFileCriterion, int maxLevel, int eventLevel, int tallyLevel)
        {
            try
            {
                _UdpLogServer = (string.IsNullOrEmpty(udpLogServer)) ? "127.0.0.1" : udpLogServer;
                _UdpPort = port;
                _EventLevel = eventLevel;
                _TallyLevel = tallyLevel;
                _MaxLevel = maxLevel;
                _NewFile = newFileCriterion;
                _Tally = new Tally();
                _Name = fileName + suffix;
                _FileBaseName = Path.Combine(dir, _Name);

                if (_LogToDatabase == 1)
                {
                    //byte[] bytes = new byte[0];
                    //bool response = SaveLogToDatabase(bytes, 0);

                    //if (!response) CreateFiles(dir, fileName, suffix);
                }
                else
                {
                    CreateFiles(dir, fileName, suffix);
                }
            }
            catch
            {
            }
        }
        
        private static void LoadSettings()
		{
            ConfigUtils.RefreshAppSettings();
            loggingServiceUrl = ConfigUtils.GetString("LoggingServiceUrl", "");
            _LogToDatabase = ConfigUtils.GetInt("LogToDatabase", -1);
		}

        public string Name { get; private set; }

        private void CreateFiles(string dir, string fileName, string suffix )
        {
            string err = null;
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string old = Path.Combine(dir, "old");
                if (!Directory.Exists(old)) Directory.CreateDirectory(old);
                string[] files = Directory.GetFiles(dir, fileName + "*");
                foreach (string file in files)
                {
                    string dest = Path.Combine(old, Path.GetFileName(file));
                    try
                    {
                        if (File.Exists(dest))
                        {
                            File.SetAttributes(dest, FileAttributes.Normal);
                            File.Delete(dest);
                        }
                        File.Move(file, dest);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            OpenFile(err);
        }

        public int MaxLevel
        {
            get { return _MaxLevel; }
            set { _MaxLevel = value; }
        }

        public void Include(string rx)
        {
            if (string.IsNullOrEmpty(rx)) return;
            _RxInclude = new Regex(rx, RegexOptions.Compiled);
        }

        public void Exclude(string rx)
        {
            if (string.IsNullOrEmpty(rx)) return;
            _RxExclude = new Regex(rx, RegexOptions.Compiled);
        }

        public static ErrorLogger GetLogger(string name)
        {
            return GetLogger(DefaultLogFolder, name, string.Empty);
        }

        public static ErrorLogger GetLogger(string dir, string name)
        {
            return GetLogger(dir, name, string.Empty);
        }

        public static ErrorLogger GetLogger(string dir, string name, string suffix)
        {
            lock (_Logs)
            {
                ErrorLogger logger;
                if (_Logs.TryGetValue(name, out logger)) return logger;
                int port;
                string s = ConfigUtils.GetString(name + "_LoggerUdpPort", "LoggerUdpPort");
                if (!string.IsNullOrEmpty(s) && int.TryParse(s, out port) && port > 0)
                {
                    string s2 = ConfigUtils.GetString(name + "_LoggerNewFile", "LoggerNewFile");
                    int num;
                    NewFile newFileCriterion = NewFile.UtcMidnight;

                    if (!string.IsNullOrEmpty(s2) && int.TryParse(s2, out num)) newFileCriterion = (NewFile)num;
                    if (string.IsNullOrEmpty(dir)) dir = DefaultLogFolder;

                    int maxLevel = ConfigUtils.GetInt(name + "_MaxLevel", "MaxLogLevel");
                    if (maxLevel == 0) maxLevel = 1;
                    int eventLevel = ConfigUtils.GetInt("EventLevel", 1);
                    int tallyLevel = ConfigUtils.GetInt("TallyLevel", 1);

                    string udpServer = ConfigUtils.GetString(name + "_LoggerUdpServer", "");

                    logger = new ErrorLogger(udpServer, port, dir, name, suffix, newFileCriterion, maxLevel, eventLevel, tallyLevel);
                }
                else
                {
                    string s2 = ConfigUtils.GetString(name + "_LoggerNewFile", "LoggerNewFile");
                    int num;
                    NewFile newFileCriterion = NewFile.UtcMidnight;

                    if (!string.IsNullOrEmpty(s2) && int.TryParse(s2, out num)) newFileCriterion = (NewFile)num;
                    if (string.IsNullOrEmpty(dir)) dir = DefaultLogFolder;

                    int maxLevel = ConfigUtils.GetInt(name + "_MaxLevel", "MaxLogLevel");
                    if (maxLevel == 0) maxLevel = 1;
                    int eventLevel = ConfigUtils.GetInt("EventLevel", 1);
                    int tallyLevel = ConfigUtils.GetInt("TallyLevel", 1);
                    logger = new ErrorLogger(dir, name, suffix, newFileCriterion, maxLevel, eventLevel, tallyLevel);
                }
                logger.Name = name;
                _Logs.Add(name, logger);
                return logger;
            }
        }

        private void OpenFile(string msg)
        {
            if (!string.IsNullOrEmpty(_Path))
            {
                try
                {
                    string move = ConfigUtils.GetString(_Name + "_MoveOldTo", "MoveOldLogsTo");
                    if (!string.IsNullOrEmpty(move))
                    {
                        if ("delete".Equals(move, StringComparison.OrdinalIgnoreCase))
                        {
                            File.Delete(_Path);
                        }
                        else
                        {
                            string fn = Path.GetFileName(_Path);
                            if (!Directory.Exists(move)) Directory.CreateDirectory(move);
                            string fn1 = Path.Combine(move, fn);
                            File.Move(_Path, fn1);
                        }
                    }
                }
                catch { }
            }
            _Path = string.Format("{0}_{1:MMdd'_'HHmmss}.txt", _FileBaseName, DateTime.UtcNow);
            _Fs = new FileStream(_Path, FileMode.Create, FileAccess.Write, FileShare.Read);
            _NewFileCreationTime = DateTime.UtcNow.Date + _24h;
            SysFormat("Start Logger. Machine: {0}, File: {1}, Mode: {2}, Zone: UTC, Level {3}", Environment.MachineName, _Path, _NewFile, _MaxLevel);
            if (!_Initialized)
            {
                Sys("Logger Initialized");
                _Initialized = true;
            }
            if (!string.IsNullOrEmpty(msg)) Sys(msg);
        }

        internal string FilePath { get { return _Path; } }

        public string Log(int level, string fmt, params object[] args)
        {
#if DEBUG
            if (fmt.Contains("BoralCmc") || fmt.ToLower().Contains("cmcagent"))
            {
                if (args != null && args.Length > 0)
                {
                    System.Diagnostics.Debug.Print(fmt, args);
                    Console.WriteLine(fmt, args);
                }
                else
                {
                    System.Diagnostics.Debug.Print(fmt);
                    Console.WriteLine(fmt);
                }
            }
#endif

            string reference = string.Empty;
            byte[] bytes;
         
            if (level > _MaxLevel) return null;
            if (_Fs == null && _UdpPort == 0 && _LogToDatabase == 0) return null;
               
            string[] symbol = {"#SYS  ", "#ERR  ", "#WARN ", "#INFO ", "#DBG  ", "#DBGV "};
            string fullMsg, sChecksum = null;
            try
            {
                if (args.Length == 0) fullMsg = fmt + Environment.NewLine;
                else fullMsg = string.Format(fmt, args) + Environment.NewLine;
            }
            catch (Exception ex)
            {
                fullMsg = ex.Message + Environment.NewLine;
            }
            string msg = fullMsg;
            if (_RxInclude != null && !_RxInclude.IsMatch(msg) || _RxExclude != null && _RxExclude.IsMatch(msg))
            {
                return null;
            }

            try
            {
                if (_LogToDatabase != 1)
                {
                    if (level <= _TallyLevel && level > 0)
                    {
                        uint checksum;
                        int tally;
                        int removed;
                        lock (_Tally) tally = _Tally.Add(msg, out checksum, out removed);
                        sChecksum = checksum.ToString("X8");
                        if (tally > 1)
                            msg = sChecksum + " (" + tally + ")" + (removed > 0 ? " -" + removed : "") +
                                  Environment.NewLine;
                        else msg = sChecksum + " := " + msg;
                    }
                }

                reference = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                bytes = Encoding.UTF8.GetBytes(symbol[level] + "|" + reference + "|" + msg);

                if (_UdpPort > 0)
                {
		            // check whether we're sending log info to another server
                    using (var udp = new UdpClient(_UdpLogServer, _UdpPort))
                    {
                        udp.Send(bytes, bytes.Length);
                    }
                    try
                    {
                        if (_Fs != null)
                        {
                            SaveLogToFileLocation(bytes);
                        }    
                    }
	                catch (Exception) { }
                }
                else if (_LogToDatabase == 1)
                {
                    try
                    {
                       // bool response = SaveLogToDatabase(bytes, level);
                        //if (!response)
                        //{
                        //    SaveLogToFileLocation(bytes);
                        //}
                    }
                    catch (Exception)
                    {
                        if (_Fs != null) SaveLogToFileLocation(bytes);
                    }
                }
                else if (_Fs != null)
                {
                    SaveLogToFileLocation(bytes);
                }

                if (level <= _EventLevel)
                {
                    const string source = "NewLogger";
                    const string log = "Application";
                    EventLogEntryType type;
                    switch (level)
                    {
                        case 1:
                            type = EventLogEntryType.Error;
                            break;
                        case 2:
                            type = EventLogEntryType.Warning;
                            break;
                        default:
                            type = EventLogEntryType.Information;
                            break;
                    }
                    if (!EventLog.SourceExists(source)) EventLog.CreateEventSource(source, log);
                    string evMsg = sChecksum == null ? fullMsg : sChecksum + " = " + fullMsg;
                    EventLog.WriteEntry(source, evMsg, type);
                }
            }
            catch
            {
            }

            return reference;
        }


        private void SaveLogToFileLocation(byte[] bytes)
        {
            lock (_FileAccess)
            {
                bool createNew;
                string note;
                switch (_NewFile)
                {
                    case NewFile.StartupOnly:
                        createNew = false;
                        note = null;
                        break;
                    case NewFile.UtcMidnight:
                        createNew = DateTime.UtcNow > _NewFileCreationTime;
                        note = "UTC midnight.";
                        break;
                    default:
                        createNew = _Fs.Length > (long)_NewFile * MB;
                        note = "Max size reached: " + _NewFile + "MB.";
                        break;
                }
                if (createNew)
                {
                    byte[] msg1 = Encoding.UTF8.GetBytes("Log ends. " + note);
                    _Fs.Write(msg1, 0, msg1.Length);
                    _Fs.Close();
                    lock (_Tally) _Tally.Reset();
                    OpenFile(null);
                }
                _Fs.Write(bytes, 0, bytes.Length);
                _Fs.Flush();
            }
        }

        private static Tracer _tracer;
        private static Tracer Tracer
        {
            get
            {
                return _tracer ?? (_tracer = new Tracer());
            }
        }

        /// <summary>
        /// Creates and return a trace log identified by a trace ID defined in the TraceLogs enum
        /// </summary>
        /// <param name="traceId"></param>
        /// <returns></returns>
        //public static NewLogger GetTraceLog(TraceLogs traceId)
        //{
        //    _tracer = _tracer ?? (_tracer = new Tracer());
        //    return _tracer.GetLog(traceId);
        //}

        /// <summary>
        /// Trace to base log using a default output format (generic Dump)
        /// </summary>
        /// <param name="traced"></param>
        public static void Trace(object traced)
        {
            Tracer.Trace(traced);
        }

        /// <summary>
        /// outputFormatter to base log
        /// </summary>
        /// <param name="s"></param>
        /// <param name="traced"></param>
        public static void TraceFormat(string s, object traced)
        {
            Tracer.TraceFormat(s, traced);
        }

        public static void Trace(TraceLogs traceId, object traced)
        {
            Tracer.Trace(traceId, traced);
        }

        public static void TraceFormat(TraceLogs traceId, string s, object traced)
        {
            Tracer.TraceFormat(traceId, s, traced);
        }

        public void Verbose(object e)
        {
            Log(5, "{0}", e);
        }

        public void Debug(object e)
        {
            Log(4, "{0}", e);
        }

        public void Info(object e)
        {
            Log(3, "{0}", e);
        }

        public void Warn(object e)
        {
            Log(2, "{0}", e);
        }

        public void Error(object e)
        {
            Log(1, "{0}", e);
        }

        public void Sys(object e)
        {
            Log(0, "{0}", e);
        }

        public void VerboseFormat(string fmt, params object[] e)
        {
            Log(5, fmt, e);
        }

        public void DebugFormat(string fmt, params object[] e)
        {
            Log(4, fmt, e);
        }

        public void InfoFormat(string fmt, params object[] e)
        {
            Log(3, fmt, e);
        }

        public void WarnFormat(string fmt, params object[] e)
        {
            Log(2, fmt, e);
        }

        public void ErrorFormat(string fmt, params object[] e)
        {
            Log(1, fmt, e);
        }

        public void SysFormat(string fmt, params object[] e)
        {
            Log(0, fmt, e);
        }
    }
}
