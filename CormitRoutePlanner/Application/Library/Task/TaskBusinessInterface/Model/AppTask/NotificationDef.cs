using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imarda360Application.Task
{
	[DataContract]
	public class NotificationDef
	{
		/// <summary>
		/// The UI sets Delivery value. Normally it's "SMTP", but can be "sFTP"   //& IM-3927
		/// </summary>
		[DataMember]
		public string Delivery { get; set; }

		[DataMember(Name = "Attach", EmitDefaultValue = false)]
		public bool AsAttachment { get; set; }

		[DataMember(Name = "Recipients")]
		private string _Recipients { get; set; }

		public void SetRecipients(string all)
		{
			_Recipients = all;
		}

		public void GetRecipients(out List<string> users, out List<string> addresses, out List<Guid> planIDs)
		{
			users = new List<string>();
			addresses = new List<string>();
			planIDs = new List<Guid>();
			if (!string.IsNullOrEmpty(_Recipients) && Delivery == "SMTP")	//# IM-3927
			{
				string[] arr = _Recipients.Split(',', ';');
				foreach (string recip in arr)
				{
					string s = recip.Trim();
					if (s.Trim() == string.Empty) continue;
					if (s.Contains('@')) addresses.Add(s);
					else if (s.Length == 36 && s[8] == '-') planIDs.Add(new Guid(s));
					else users.Add(s);
				}
			}
		}

		//& IM-3927
		public void GetFTPDetails(out string server, out string port, out string username, out string password, out string psk, out string destinationPath)
		{
			server = "";
			port = "";
			username = "";
			password = "";
			psk = "";
			destinationPath = "";

			if (!string.IsNullOrEmpty(_Recipients) && Delivery.ToUpper() == "SFTP")
			{
				string[] arr = _Recipients.Split(',', ';');
				if (arr.Length >= 6)
				{
					server = arr[0];
					port = arr[1];
					username = arr[2];
					password = arr[3];
					psk = arr[4];
					destinationPath = arr[5];
				}
			}
		}
		//. IM-3927
        public void GetFTPDetails(out string recipients)
        {
            recipients = _Recipients;
        }
		[DataMember]
		public byte Priority { get; set; }

		[DataMember]
		public string Comment { get; set; }


		public override string ToString()
		{
			return string.Format("NotDef({0} {1} {2})", Delivery, Comment, Priority);
		}
	}
}
