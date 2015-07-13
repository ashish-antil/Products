using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class Client : FullBusinessEntity
	{
		#region Constructor
		public Client()
		{
		}
		#endregion

		[DataMember]
		public string ClientId { get; set; }

		[DataMember]
		public string ClientSecret { get; set; }

		[DataMember]
		public string ClientName { get; set; }

		[DataMember]
		public byte ClientType { get; set; }

		[DataMember]
		public string ClientUri { get; set; }

		[DataMember]
		public DateTime ValidFrom { get; set; }

		[DataMember]
		public DateTime ValidUntil { get; set; }

		[DataMember]
		public string ContactInfo { get; set; }

		[DataMember]
		public string Legal { get; set; }

		[DataMember]
		public string Notes { get; set; }

		[DataMember]
		public string SupportedFlows { get; set; }

		[DataMember]
		public byte ConsentQuery { get; set; }

		[DataMember]
		public byte TokenSigning { get; set; }

		[DataMember]
		public string RegisteredUris { get; set; }

		[DataMember]
		public byte AccessTokenType { get; set; }

		[DataMember]
		public int AccessTokenExpiration { get; set; }

		[DataMember]
		public int AuthorizationCodeExpiration { get; set; }

		[DataMember]
		public int IdentityTokenExpiration { get; set; }

		[DataMember]
		public byte IssueRefreshToken { get; set; }

		[DataMember]
		public byte RefreshTokenUsage { get; set; }

		[DataMember]
		public byte RefreshTokenExpiration { get; set; }

		[DataMember]
		public int AbsoluteRefreshTokenExpiration { get; set; }

		[DataMember]
		public int SlidingRefreshTokenExpiration { get; set; }


#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ClientId = DatabaseSafeCast.Cast<string>(dr["ClientId"]);
			ClientSecret = DatabaseSafeCast.Cast<string>(dr["ClientSecret"]);
			ClientName = DatabaseSafeCast.Cast<string>(dr["ClientName"]);
			ClientType = DatabaseSafeCast.Cast<byte>(dr["ClientType"]);
			ClientUri = DatabaseSafeCast.Cast<string>(dr["ClientUri"]);
			ContactInfo = DatabaseSafeCast.Cast<string>(dr["ContactInfo"]);
			Legal = DatabaseSafeCast.Cast<string>(dr["Legal"]);
			Notes = DatabaseSafeCast.Cast<string>(dr["Notes"]);
			SupportedFlows = DatabaseSafeCast.Cast<string>(dr["SupportedFlows"]);
			ConsentQuery = DatabaseSafeCast.Cast<byte>(dr["ConsentQuery"]);
			TokenSigning = DatabaseSafeCast.Cast<byte>(dr["TokenSigning"]);
			RegisteredUris = DatabaseSafeCast.Cast<string>(dr["RegisteredUris"]);
			AccessTokenType = DatabaseSafeCast.Cast<byte>(dr["AccessTokenType"]);
			AccessTokenExpiration = DatabaseSafeCast.Cast<int>(dr["AccessTokenExpiration"]);
			AuthorizationCodeExpiration = DatabaseSafeCast.Cast<int>(dr["AuthorizationCodeExpiration"]);
			IdentityTokenExpiration = DatabaseSafeCast.Cast<int>(dr["IdentityTokenExpiration"]);
			IssueRefreshToken = DatabaseSafeCast.Cast<byte>(dr["IssueRefreshToken"]);
			RefreshTokenUsage = DatabaseSafeCast.Cast<byte>(dr["RefreshTokenUsage"]);
			RefreshTokenExpiration = DatabaseSafeCast.Cast<byte>(dr["RefreshTokenExpiration"]);
			AbsoluteRefreshTokenExpiration = DatabaseSafeCast.Cast<int>(dr["AbsoluteRefreshTokenExpiration"]);
			SlidingRefreshTokenExpiration = DatabaseSafeCast.Cast<int>(dr["SlidingRefreshTokenExpiration"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

			ValidFrom = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["ValidFrom"]));
			ValidUntil = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["ValidUntil"]));

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}