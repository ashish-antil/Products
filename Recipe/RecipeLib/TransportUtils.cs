using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace RockBottom.CommunicationLib
{
	public static class TransportUtils
	{
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
			var c = att.ContentDisposition;
			c.CreationDate = File.GetCreationTime(path);
			c.DispositionType = DispositionTypeNames.Attachment;
			c.FileName = Path.GetFileName(path);
			mm.Attachments.Add(att);
			return mm;
		}
	}


}
