using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Imarda.Lib;

namespace ImardaReportBusiness
{
	partial class Report
	{
		[DataMember]
		public string LoginUser { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public string Domain { get; set; }

		[DataMember]
		public string BaseUrl { get; set; }


		public void GetPathAndHistoryID(out string path, out string historyID)
		{
			string[] parts = SnapshotName.Split('|');
			path = parts[0];
			historyID = parts[1];
		}

		public string GetReportURL(string host)
		{
			if (!host.StartsWith("http")) host = string.Format(@"http://{0}/ReportServer", host);
			var parts = SnapshotName.Split('|');
			return string.Format(@"{0}?{1}&rs:Command=Render&rs:Snapshot={2}",
				host, StringUtils.UrlEncode(parts[0]), StringUtils.UrlEncode(parts[1]));
		}

		public string GetReportFormatURL(string serverBasePath, string format, out string name)
		{
			var parts = SnapshotName.Split('|');
			name = parts[1];
		    string _format = format;

		    switch (format.ToLower())
		    {
                case "xls2csv":
                case "xls":
		            _format = "Excel";
		            break;
                default:
		            break;
		    }

			return string.Format(@"{0}?{1}&rs:Command=Render&rs:Format={2}&rs:Snapshot={3}",
				serverBasePath, //0
				StringUtils.UrlEncode(parts[0]), //1
                _format, //2    
				StringUtils.UrlEncode(name) //3
				);
		}
	}
}
