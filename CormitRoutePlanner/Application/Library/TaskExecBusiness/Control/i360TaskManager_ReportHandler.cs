using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;
using Imarda360.Infrastructure.ConfigurationService;

using ImardaConfigurationBusiness;
using ImardaNotificationBusiness;
using ImardaCRMBusiness;


namespace Cormit.Application.RouteApplication.Task
{
    /// <summary>
    /// This partial class handles the Reports.
    /// </summary>
    partial class i360TaskManager
    {
        //private static Regex _rxTemplateParameter = new Regex(@"<([A-Za-z_]\w*)>", RegexOptions.Compiled);

        private bool _CleanUpAttachments;
        private string _DownloadFolder;
        private string _ReportHost;
        private ICredentials _ReportServerLogin;


        /// <summary>
        /// Create the report objects. Do not download the report yet.
        /// </summary>
        /// <param name="repTask"></param>
        /// <param name="companyID"></param>
        /// <param name="reportName">The name of the report, valid filename</param>
        /// <returns>the report server URL where to download the report</returns>
        private Uri RunReport(ReportTask repTask, Guid companyID, out string reportName, out string ext)
        {
            _Log.InfoFormat("Run report {0}", repTask.ID);

            reportName = "";
            ext = "";
            ReportDef rdef = repTask.Definition;

            string pref = String.Empty;

            IImardaConfiguration service1 = ImardaProxyManager.Instance.IImardaConfigurationProxy;
            ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                                                            {
                                                                channel = service1 as IClientChannel;

                                                                Guid unitSys = rdef.UnitSystemID;
                                                                _Log.InfoFormat("unitSys = {0}", unitSys);
                                                                if (unitSys == CultureHelper.MetricUnitSystemID
                                                                        || unitSys == CultureHelper.USCustomaryUnitSystemID
                                                                        || unitSys == CultureHelper.ImperialUnitSystemID)
                                                                {
                                                                    _Log.Info("Specific Unit System selected");
                                                                    Guid countryID = CultureIDs.Instance.GetCountryGuid(CultureHelper.GetRegion(rdef.Locale));
                                                                    _Log.InfoFormat("countryID = {0}", countryID);
                                                                    var req1 = new ConfigRequest(CultureHelper.RegionMeasurementUnitsID, null, unitSys,
                                                                                                                             countryID);
                                                                    GetItemResponse<ConfigValue> resp1 = service1.GetConfigValue(req1);
                                                                    ErrorHandler.CheckItem(resp1);
                                                                    _Log.InfoFormat("prefs = {0} {1}", resp1.Item.UID, resp1.Item.Value);
                                                                    pref = resp1.Item.As<string>();
                                                                }
                                                                else
                                                                {
                                                                    _Log.Info("Company/User Unit System selected");
                                                                    Guid personID = (unitSys == Guid.Empty || unitSys == new Guid("B820DAA2-6059-43EE-998B-7293A2CF4B6F")) ? Guid.Empty : repTask.TaskOwnerID;
                                                                    _Log.InfoFormat("personID = {0}", personID);
                                                                    SimpleResponse<string> resp1 =
                                                                        service1.GetCulturePreferences(new GenericRequest(personID, companyID));
                                                                    ErrorHandler.Check(resp1);
                                                                    pref = resp1.Value;
                                                                }
                                                            });
            _Log.InfoFormat("pref = {0}", pref);

            //ReportType linkedReportType = null;

            //IImardaReport service2 = ImardaProxyManager.Instance.IImardaReportProxy;
            //ChannelInvoker.Invoke(delegate(out IClientChannel channel)
            //    {
            //        channel = service2 as IClientChannel;

            //        GetItemResponse<ReportType> resp2 = service2.GetReportType(new IDRequest(rdef.ReportTypeID));
            //        ErrorHandler.CheckItem(resp2);
            //        ReportType reportType = resp2.Item;

            //        string context = rdef.ReportContext(companyID, repTask.GetTimeZoneInfo(), DateTime.UtcNow,
            //                                                                                pref);
            //        _Log.InfoFormat("Report Parameters = {0}", context);
            //        _Log.InfoFormat("Period = {0} ~ {1}", rdef.PeriodBegin, rdef.PeriodEnd);
            //        var parameters = new ParamsForCreateLinkedReportType
            //                                            {
            //                                                CompanyID = companyID,
            //                                                UserID = repTask.TaskOwnerID,
            //                                                Name = reportType.Name,
            //                                                Version = reportType.Version,
            //                                                OwnerID = repTask.TaskOwnerID,
            //                                                ReportNumber = null,
            //                                                ReportParameters = context,
            //                                                Description = string.Format(
            //                                                    // new Locale(rdef.Locale),
            //                                                    "{0} {1:dd MMM yyyy HH:mm} - {2:dd MMM yyyy HH:mm}",
            //                                                    rdef.Description, rdef.PeriodBegin, rdef.PeriodEnd)
            //                                            };

            //        resp2 = service2.CreateLinkedReportType(parameters.AsGenericRequest());
            //        ErrorHandler.CheckItem(resp2);
            //        _Log.InfoFormat("Create Linked Report", resp2.Item);
            //        linkedReportType = resp2.Item;
            //    });

            //string name = null;
            //string url = String.Empty;
            //IImardaReport service3 = ImardaProxyManager.Instance.IImardaReportProxy;
            //ChannelInvoker.Invoke(delegate(out IClientChannel channel)
            //                                                {
            //                                                    _Log.InfoFormat("Create snapshot {0}", linkedReportType.ID);
            //                                                    channel = service3 as IClientChannel;
            //                                                    channel.OperationTimeout = TimeSpan.FromMinutes(5);
            //                                                    var req = new GenericRequest(linkedReportType.ID);
            //                                                    GetItemResponse<Report> resp3 = service3.CreateSnapshot(req);
            //                                                    _Log.InfoFormat("Result {0}", resp3);
            //                                                    ErrorHandler.CheckItem(resp3);
            //                                                    Report report = resp3.Item;
            //                                                    _Log.DebugFormat("Original Format {0}", rdef.RenderFormat.ToString());
            //                                                    url = report.GetReportFormatURL(_ReportHost, rdef.RenderFormat.ToString(), out name);
            //                                                    _Log.DebugFormat("Format After Get ReportUrl{0}", rdef.RenderFormat.ToString());
            //                                                    name = MakeFileName(name, companyID, rdef.RenderFormat);
            //                                                    _Log.InfoFormat("Report URL {0} => [{1}] with Render Format => {2}", url, name, rdef.RenderFormat.ToString()); 
            //                                                });
            //reportName = name;
            //ext = rdef.RenderFormat.ToString();
            //return String.IsNullOrEmpty(url) ? null : new Uri(url);
            var tempuri = string.Empty;
            return String.IsNullOrEmpty(tempuri) ? null : new Uri(tempuri);
            //return new Uri();
        }

        /// <summary>
        /// Turn the snapshort name into a valid filename by removing some chars adding ext + prefix.
        /// </summary>
        /// <param name="reportSnapshotName">e.g. 2010-12-09T16:01:00  (url escape)</param>
        /// <param name="fmt">renderformat by default is extension, but there are some exceptions</param>
        /// <returns>e.g. i360_30101209_160100.pdf</returns>
        private static string MakeFileName(string reportSnapshotName, Guid companyID, RenderFormat fmt)
        {
            string ext;
            switch (fmt)
            {
                case RenderFormat.None:
                    ext = "att";
                    break;
                case RenderFormat.Excel:
                    ext = "xls";
                    break;
                case RenderFormat.Xls2Csv:
                    ext = "xls";
                    break;
                case RenderFormat.Word:
                    ext = "doc";
                    break;
                case RenderFormat.Image:
                    ext = "tiff";
                    break;
                default:
                    ext = fmt.ToString().ToLowerInvariant();
                    break;
            }
            // 2010-12-09T16:01:00 -> i360_20101209_160100.pdf
            string name = "i360_" + companyID.ToString().Substring(0, 4) + GetNextSeqNum() + "_" + reportSnapshotName.Replace(":", "").Replace("-", "").Replace("T", "_") + "." + ext;
            return name;
        }

        private static int _SeqNum;

        private static string GetNextSeqNum()
        {
            if (++_SeqNum > 999) _SeqNum = 0;
            return _SeqNum.ToString("000");
        }

        private void InfoFormat(string text, params object[] values)
        {
            if (_Log != null)
                _Log.InfoFormat(text, values);
        }

        private void ErrorFormat(string text, params object[] values)
        {
            if (_Log != null)
                _Log.ErrorFormat(text, values);
        }

        /// <summary>
        /// Download the report from the 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        internal string DownloadReport(Uri uri, string filename, string ext)
        {
            _DownloadFolder = ConfigUtils.GetString("DownloadFolder") ?? @"C:\";
            string path = Path.Combine(_DownloadFolder, filename);
            TransportUtils.Download(uri, _ReportServerLogin, path);
            this.InfoFormat("Uri {0}, Logs: {1}, Path:{2}", uri, _ReportServerLogin, path);

            //check if ext was "xls2csv" 
            //WorkAround for Reports that should be in csv but still needs some formatting
            if (ext.ToLower() == "xls2csv")
            {
                this.InfoFormat("Xls to Csv Hack (1) Activated for {0}, Logs: {1}, Path:{2}", uri, _ReportServerLogin, path);

                try
                {
                    path = i360TaskManager.ApplyCsvHack(path, _DownloadFolder);
                }
                catch (Exception ex)
                {
                    // handle exception
                    this.ErrorFormat("Expection Occurred while exporting Excel to CSV. Details {0}", ex);
                }
            }
            return path;
        }

        /// <summary>
        /// Creates a Boral CMC specific CSV file from an excel file with DDW information
        /// </summary>
        /// <param name="sourceXlsFile">Source excel file</param>
        /// <param name="outputFolder">Destination folder for the resulting CSV file</param>
        /// <returns></returns>
        internal static string ApplyCsvHack(string sourceXlsFile, string outputFolder)
        {
            var finalFile = sourceXlsFile;

            const string xlsConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1\"";
            var fileName = Path.GetFileNameWithoutExtension(sourceXlsFile);
            
            if (!string.IsNullOrEmpty(fileName))
            {
                // the final file that will be returned is the hacked CSV file
                finalFile = Path.Combine(outputFolder, fileName + ".csv");

                // delete the CSV file if it already exists
                if (File.Exists(finalFile)) File.Delete(finalFile);

                using (var conn = new OleDbConnection(string.Format(xlsConnString, sourceXlsFile)))
                {
                    conn.Open();

                    string[] sheetNames = conn.GetSchema("Tables").AsEnumerable().Select(a => a["TABLE_NAME"].ToString()).ToArray();

                    foreach (string sheetName in sheetNames)
                    {
                        using (var sw = new StreamWriter(File.Create(finalFile), Encoding.Unicode))
                        {
                            using (var ds = new DataSet())
                            {
                                using (var adapter = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", sheetName), conn))
                                {
                                    adapter.Fill(ds);

                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        var dr = ds.Tables[0].Rows[i];

                                        string[] cells = dr.ItemArray.Select(a => a.ToString()).ToArray();

                                        // ensure this is not an empty row before we write it out to the file
                                        if (string.Join(",", cells).Replace(",","").Trim() == string.Empty)
                                        {
                                            continue;
                                        }

                                        // check whether or not the current line is for a DDW header
                                        if (cells[0].ToLower() == "name" && cells[1].ToLower() == "payrollnumber")
                                        {
                                            // use only the required DDW header columns (some columns will be blank)
                                            sw.WriteLine("{0}", string.Join(",", cells.Take(12)));   

                                            // also write the DDW header values ignoring trailing commas
                                            i++;
                                            dr = ds.Tables[0].Rows[i];
                                            cells = dr.ItemArray.Select(a => a.ToString()).ToArray();
                                            sw.WriteLine("{0}", string.Join(",", cells.Take(12)));   
                                        }
                                        else
                                        {
                                            sw.WriteLine("{0}", string.Join(",", cells));      
                                        }
                                    }
                                }
                            }
                        }

                        var strQ = File.ReadAllText(finalFile);
                        File.WriteAllText(finalFile, strQ);
                    }
                }
            }

            return finalFile;
        }


        private void NotifyRecipients(ReportTask task, string path)
        {
            _Log.Info("Notify Recipients");

            NotificationDef ndef = task.Notification;
            ReportDef rdef = task.Definition;

            //# IM-3927
            List<string> users = new List<string>();
            List<string> addresses = new List<string>();
            List<Guid> planIDs = new List<Guid>();
            string server = "";
            string port = "22";
            string username = "";
            string password = "";
            string psk = "";
            string destinationPath = "";

            switch (ndef.Delivery.ToUpper())
            {
                case "SMTP":
                    ndef.GetRecipients(out users, out addresses, out planIDs); // user IDs currently not used
                    if (users.Count == 0 && addresses.Count == 0 && planIDs.Count == 0)
                    {
                        Attention(new Guid("79b09d0b-0a0a-badd-a0ef-386d5dedf1fc"), "No recipients for ReportTask.ID=" + task.ID);
                        return; // no notifications required
                    }
                    break;

                case "SFTP":
                    ndef.GetFTPDetails(out server, out port, out username, out password, out psk, out destinationPath);
                    if (server == "" || username == "" || password == "")  // || destinationFolder == "")
                    {
                        Attention(new Guid("79b09d0b-0a0a-badd-a0ef-386d5dedf1fc"), "No sFTP details for ReportTask.ID=" + task.ID);
                        return; // no notifications required
                    }
                    break;
            }
            //. IM-3927


            // Download report
            string attachment = null;
            if (!String.IsNullOrEmpty(path))
            {
                try
                {
                    attachment = Path.GetFileName(path) + "|" + Convert.ToBase64String(File.ReadAllBytes(path));
                    _Log.InfoFormat("Attachment {0} bytes, {1} chars", new FileInfo(path).Length, attachment.Length);
                }
                catch (Exception ex)
                {
                    _Log.ErrorFormat("Error reading attachment file {0}: {1}", path, ex.Message);
                }
            }
            //Priority: have to make string to pass to service methods
            byte priority = task.Notification.Priority;

            StringBuilder sb = GetParametersForTemplate(task);

            //# IM-3927
            switch (ndef.Delivery.ToUpper())
            {
                case "SMTP":
                    // Send notification plans
                    if (planIDs.Count > 0)
                    {
                        _Log.InfoFormat("Send {0} Notification Plan(s)", planIDs.Count);

                        // The Comment field contains a message that may contain <...> or $(...) placeholders for parameters
                        //    therefore we have to instantiate it here to create a message.
                        // Add the instantiated message to the list of parameters
                        //    This can be expanded in a NotificationItem template where <Message> or $(Message) is found,
                        //    but it is also the body for single Emails
                        string message = FillInTemplate(rdef.Locale, task.TimeZoneID, task.Notification.Comment, sb.ToString());
                        sb.AppendKV("Message", message);
                        string allParameters = sb.ToString();
                        _Log.InfoFormat("All parameters for notification plan: {0}", allParameters);

                        IImardaCRM crmService = ImardaProxyManager.Instance.IImardaCRMProxy;
                        ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                        {
                            channel = crmService as IClientChannel;
                            foreach (Guid planID in planIDs)
                            {
                                var crmreq = new GenericRequest(planID, task.CompanyID, allParameters, task.TimeZoneID,
                                                                                                attachment, priority);
                                BusinessMessageResponse crmresp = crmService.SendNotificationPlan(crmreq);
                                _Log.InfoFormat("After send notification plan {0} -> {1}", planID, crmresp.Status);
                            }
                        });
                    }

                    // Send to addresses that were entered in the UI
                    if (addresses.Count > 0)
                    {
                        _Log.InfoFormat("Send {0} Notification Email(s)", addresses.Count);
                        string allParameters = sb.ToString();
                        // we don't know the user names, therefore use the address as user name; (we must fill in unique address)
                        string recipients = String.Join("||", addresses.Select(addr => addr + "|" + addr).ToArray());

                        var param = new string[11];

                        string emptyID = Guid.Empty.ToString();
                        param[0] = task.CompanyID.ToString();
                        param[1] = emptyID;
                        param[2] = task.Notification.Comment; // template to be instantiated in the SendEmail
                        param[3] = "i360 - Report - " + rdef.Description; // subject
                        param[4] = attachment;
                        param[5] = recipients;
                        param[6] = ""; // cc
                        param[7] = ""; // bcc
                        param[8] = allParameters; // parameters that go optionally into the template
                        param[9] = task.TimeZoneID;
                        param[10] = priority.ToString();


                        IImardaNotification notifService = ImardaProxyManager.Instance.IImardaNotificationProxy;
                        ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                        {
                            channel = notifService as IClientChannel;

                            var nreq = new GenericRequest(Guid.Empty, param);
                            var nresp = notifService.SendEmail(nreq);

							//IM-5884 now checks response from SendSingle and returns it
							if (!nresp.Status)
							{
								_Log.WarnFormat("{0} - Error on sending email: {1}", MethodBase.GetCurrentMethod().Name, nresp.Status);
							}
							_Log.InfoFormat("After send email to {0} : {1} -> {2}", recipients ?? "recipients is null", param[3], nresp.Status);
                        });
                    }
                    break;

                case "SFTP":
                    if (server == "" || username == "" || password == "")  // || destinationPath == "")
                        break;

                    _Log.InfoFormat("Send {0} Notification via sFTP", server + "," + username);
                    var ftpParam = new string[10];

                    string emptyUserID = Guid.Empty.ToString();
                    ftpParam[0] = task.CompanyID.ToString();
                    ftpParam[1] = emptyUserID;
                    ftpParam[2] = server;
                    ftpParam[3] = port;
                    ftpParam[4] = username;
                    ftpParam[5] = password;
                    ftpParam[6] = psk;
                    ftpParam[7] = destinationPath;
                    ftpParam[8] = attachment;
                    ftpParam[9] = priority.ToString();

                    IImardaNotification notifyService = ImardaProxyManager.Instance.IImardaNotificationProxy;
                    ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                    {
                        channel = notifyService as IClientChannel;
                        var nreq = new GenericRequest(Guid.Empty, ftpParam);
                        BusinessMessageResponse nresp = notifyService.SendsFTP(nreq);
                        _Log.InfoFormat("After send report via sFTP to {0} : {1} -> {2} {3}", server, username, nresp.Status, rdef.Description);
                    });
                    break;
            }

            if (!String.IsNullOrEmpty(path))
            {
                try
                {
                    if (_CleanUpAttachments)
                    {
                        File.Delete(path);
                        _Log.Info("Attachment deleted");
                    }
                }
                catch (Exception ex)
                {
                    _Log.ErrorFormat("Error deleting attachment file {0}: {1}", path, ex.Message);
                }
            }

            //. IM-3927
        }

        private static StringBuilder GetParametersForTemplate(ReportTask task)
        {
            ReportDef rdef = task.Definition;
            NotificationDef ndef = task.Notification;

            Dictionary<string, string> map = rdef.ReportParameters.KeyStringMap(false);
            IDictionary newmap = new HybridDictionary(false);
            foreach (string key in map.Keys)
                newmap[key + '$'] = map[key]; // $=indicate that value has to be interpreted as string
            string typedReportParameters = newmap.KeyValueString();
            _Log.InfoFormat("Typed Report Parameters = {0}", typedReportParameters);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(task.TimeZoneID);

            StringBuilder sb = new StringBuilder()
                .AppendKV("StartTime", TimeZoneInfo.ConvertTimeToUtc(task.StartTime, tzi))
                .AppendKV("Priority", ndef.Priority)
                .AppendKV("FirstDay", TimeZoneInfo.ConvertTimeToUtc(rdef.PeriodBegin, tzi), "z")
                .AppendKV("LastDay", TimeZoneInfo.ConvertTimeToUtc(rdef.PeriodEnd.AddHours(-3).Date, tzi), "z")
                // make sure before midnight, then take Date
                .AppendKV("Offset", rdef.Offset)
                .AppendKV("Scale", rdef.TimeScale.ToString())
                .AppendKV("Locale", rdef.Locale)
                .AppendKV("RepDesc", rdef.Description ?? "")
                .AppendKV("Format", rdef.RenderFormat.ToString())
                .AppendKV("UnitSys", CultureHelper.EnglishNameExtended(rdef.UnitSystemID))
                .Append("||").Append(typedReportParameters); // report specific parameters are concatenated here
            return sb;
        }


        /// <summary>
        /// Fill in the template, replace identifiers in the template in angular brackets by $(identifier).
        /// Flatten the arrays in the data to simple identifiers
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <param name="template"></param>
        /// <param name="locale"></param>
        /// <param name="typedData"></param>
        /// <returns></returns>
        private static string FillInTemplate(string locale, string timeZoneId, string template, string typedData)
        {
            if (String.IsNullOrEmpty(template)) return String.Empty;
            //template = _rxTemplateParameter.Replace(template, @"$$($1)");
            template = template.Replace("<br>", "\r\n").Replace("&nbsp;", " ");
            var culture = new CultureInfo(locale);
            var mfi = new MeasurementFormatInfo(culture.NumberFormat);
            var ifp = new ImardaFormatProvider(culture, mfi, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
            IDictionary args = EAHelper.MakeFormattedValues(typedData, ifp);
            string msg = new ConfigTemplate(template).Instantiate(args);
            return msg;
        }


    }

}
