using System.Text;
using System.Xml;

namespace Imarda.Lib
{
	/// <summary>
	/// Interface implemented by base response objects from services.
	/// ErrorHandler works with this interface.
	/// </summary>
	public interface IServiceMessageResponse
	{
		bool Status { get; set; }
		string StatusMessage { get; set; }
		string ErrorCode { get; set; }
		object Payload { get; }
	}


	public static class ServiceMessageHelper
	{
		/// <summary>
		/// Allows status to be copied e.g. from lower level service to higher level service response.
		/// </summary>
		public static void Copy(IServiceMessageResponse from, IServiceMessageResponse to)
		{
			to.Status = from.Status;
			to.StatusMessage = from.StatusMessage;
			to.ErrorCode = from.ErrorCode;
		}

		public static string ToString(IServiceMessageResponse resp)
		{
			var sb = new StringBuilder();
			WriteXml(resp, sb, null);
			return sb.ToString();
		}

		public static string ToString(IServiceMessageResponse resp, string details)
		{
			var sb = new StringBuilder();
			WriteXml(resp, sb, details);
			return sb.ToString();
		}

		public static void WriteXml(IServiceMessageResponse resp, StringBuilder sb, string details)
		{
			XmlWriter w = XmlWriter.Create(sb, new XmlWriterSettings { IndentChars = "\t", Indent = true });
			if (resp.Status)
			{
				w.WriteStartElement("Response");
			}
			else
			{
				w.WriteStartElement("Error");
				w.WriteElementString("ErrorCode", resp.ErrorCode);
			}
			w.WriteElementString("StatusMessage", resp.StatusMessage != null ? resp.StatusMessage.Replace('\0', '.') : "");
			if (!string.IsNullOrEmpty(details)) w.WriteElementString("Details", details);
			w.WriteEndElement();
			w.Close();
		}

		public static bool IsSuccess(IServiceMessageResponse resp)
		{
			if (resp == null || resp.Status == false) return false;
			return resp.Payload != null || resp.GetType().BaseType == typeof (object); // top level has null Payload
		}

		public static bool IsSessionProblem(IServiceMessageResponse appResponse)
		{
			return (appResponse != null
			        && appResponse.ErrorCode != null
			        && appResponse.ErrorCode.StartsWith("APxx|Session|"));
		}
	}
}