using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImardaReportBusiness;
using FernBusinessBase;
using System.ServiceModel;
using System.Threading;

namespace ConnectionTest
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void Connect_Click(object sender, EventArgs e)
		{
			_Result.Clear();
			var req = new GenericRequest(
				Guid.Empty,
				_MicrosoftReportingSvc.Text.Trim(),
				_Network.Checked ? "network" : "default",
				_User.Text.Trim(),
				_Password.Text.Trim(),
				_Domain.Text.Trim()
				);
			Cursor = Cursors.WaitCursor;
			ThreadPool.QueueUserWorkItem(Connect, req);
		}

		private void Connect(object o)
		{
			string msg = string.Empty;
			try
			{
				var ep = new EndpointAddress(_ImardaReportSvc.Text.Trim());
				ChannelFactory<IImardaReport> imardaReportingService = new ChannelFactory<IImardaReport>("ReportingTcpEndpoint", ep);
				var channel = imardaReportingService.CreateChannel();

				BusinessMessageResponse resp = channel.Connect((GenericRequest)o);
				if (resp.StatusMessage.Contains("The role 'nonexistent' cannot be found.")) msg = "Success";
				else msg = resp.StatusMessage;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			SetResult(msg);
		}

		private delegate void SetResultDelegate(string s);
		private void SetResult(string s)
		{
			if (InvokeRequired) Invoke(new SetResultDelegate(SetResult), s);
			else
			{
				_Result.Text = s;
				Cursor = Cursors.Default;
			}
		}
	}
}
