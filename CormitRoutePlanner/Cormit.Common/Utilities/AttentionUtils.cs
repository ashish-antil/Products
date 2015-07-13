using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Imarda.Lib
{
	public static class AttentionUtils
	{
		private static readonly string _IPAddress = ConfigUtils.GetString("AttentionUdpAddress") ?? "127.0.0.1";
		private static readonly int _Port = ConfigUtils.GetInt("AttentionUdpPort", 0xBADD); /*47837*/

		private static readonly object _AttentionSync = new object();

		public static void SendHeartbeat(Guid id)
		{
			try
			{
				var bytes = id.ToByteArray();
				new UdpClient(_IPAddress, _Port).Send(bytes, bytes.Length);
			}
			catch
			{
			}
		}

		private static string SendAttentionInfo(string header, Guid issueID, string text, object[] args)
		{
			lock (_AttentionSync)
			{
				var msg = string.Format(header + " " + issueID + " " + text, args);
				Execute.Async(
					delegate
					{
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(msg);
							new UdpClient(_IPAddress, _Port).Send(bytes, bytes.Length);
						}
						catch
						{
						}
					});
				return msg;
			}
		}


		public static string Attention(Guid issueID, string text, params object[] args)
		{
			return SendAttentionInfo("ATTENTION!", issueID, text, args);
		}

		public static string CancelAttention(Guid issueID, string text, params object[] args)
		{
			return SendAttentionInfo("ATTENTION END", issueID, text, args);
		}
	}
}
