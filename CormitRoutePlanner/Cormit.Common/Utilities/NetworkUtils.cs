using System.Linq;
using System.Net;

namespace Imarda.Lib
{
	public class DNSUtil
	{
		private static string[] _LocalAddresses;
		private static string _LocalHostName;

		/// <summary>
		/// Get machine address information. Caches the information if hostName == null.
		/// </summary>
		/// <param name="hostName">host name to return the list of IP addresses for, or null for local machine </param>
		/// <returns>list of IP addresses</returns>
		public static string[] GetAddress(ref string hostName)
		{
			if (hostName == null || _LocalHostName == hostName)
			{
				if (_LocalHostName == null)
				{
					hostName = Dns.GetHostName();
					_LocalHostName = hostName;
					IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
					_LocalAddresses = ipEntry.AddressList.Select(a => a.ToString()).ToArray();
				}
				else
				{
					hostName = _LocalHostName;
				}
				return _LocalAddresses;
			}
			else
			{
				IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
				return ipEntry.AddressList.Select(a => a.ToString()).ToArray();
			}
		}
	}
}