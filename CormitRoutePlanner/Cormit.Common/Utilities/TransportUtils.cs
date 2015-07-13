using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Imarda.Lib
{
	public static class TransportUtils
	{
		/// <summary>
		/// Make a web request to the given URL using the given credentials, and download the response into a file.
		/// </summary>
		/// <param name="uri">download url</param>
		/// <param name="cred">credentials to access url</param>
		/// <param name="path">file path</param>
		public static void Download(Uri uri, ICredentials cred, string path)
		{
			var request = (HttpWebRequest) WebRequest.Create(uri);
			request.Timeout = 60*1000;
			request.Method = "GET";
			request.KeepAlive = true;
			request.Credentials = cred;

			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse) request.GetResponse();
				using (Stream rcvd = response.GetResponseStream())
				{
					using (FileStream store = File.Create(path))
					{
						int n = 64*1024;
						var buf = new byte[n];
						while ((n = rcvd.Read(buf, 0, n)) > 0)
						{
							store.Write(buf, 0, n);
						}
					}
				}
			}
			finally
			{
				if (response != null) response.Close();
			}
		}


		/// <summary>
		/// Extension method to add a file attachment to a MailMessage. Set the creation time and filename.
		/// </summary>
		/// <example>smtp.Send(mm.Attach(file1).Attach(file2));</example>
		/// <param name="mm">the message to be sent, may already have attachments</param>
		/// <param name="path">path of file to attach</param>
		/// <returns>the same MailMessage, but with a new attachment</returns>
		public static MailMessage Attach(this MailMessage mm, string path)
		{
			var att = new Attachment(path, MediaTypeNames.Application.Octet);
			ContentDisposition c = att.ContentDisposition;
			c.CreationDate = File.GetCreationTime(path);
			c.DispositionType = DispositionTypeNames.Attachment;
			c.FileName = Path.GetFileName(path);
			mm.Attachments.Add(att);
			return mm;
		}
	}
}