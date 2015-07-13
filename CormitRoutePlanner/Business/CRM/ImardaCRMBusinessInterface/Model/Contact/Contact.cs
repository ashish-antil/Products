using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaCRMBusiness
{
	[DataContract]
	public class Contact : FullBusinessEntity
	{
		public enum ContactType
		{
			Contact = 0,
			Customer = 1,
			Supplier = 2,
			ContactPerson = 3
		}

		public Contact()
		{
		}

		public const int NameMaxLen = 50;
		[ValidLength(0, NameMaxLen)]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Guid ContactPersonID { get; set; }

		public const int StreetAddressMaxLen = 50;
		[ValidLength(-1, StreetAddressMaxLen)]
		[DataMember]
		public string StreetAddress { get; set; }

		public const int SuburbMaxLen = 50;
		[ValidLength(-1, SuburbMaxLen)]
		[DataMember]
		public string Suburb { get; set; }

		public const int StateMaxLen = 150;
		[ValidLength(-1, StateMaxLen)]
		[DataMember]
		public string State { get; set; }

		[DataMember]
		public string Country { get; set; }

		public const int PhoneMaxLen = 50;
		[ValidLength(-1, PhoneMaxLen)]
		[DataMember]
		public string Phone { get; set; }

		public const int MobileMaxLen = 50;
		[ValidLength(-1, MobileMaxLen)]
		[DataMember]
		public string Mobile { get; set; }

		public const int FaxMaxLen = 50;
		[ValidLength(-1, FaxMaxLen)]
		[DataMember]
		public string Fax { get; set; }

		public const int EmailMaxLen = 250;
		[ValidLength(-1, EmailMaxLen)]
		[DataMember]
		public string Email { get; set; }

		public const int JobTitleMaxLen = 50;
		[ValidLength(-1, JobTitleMaxLen)]
		[DataMember]
		public string JobTitle { get; set; }

		[DataMember]
		public string DefaultLocation { get; set; }

		[DataMember]
		public ContactType Type { get; set; }

		[DataMember]
		public int ClientType { get; set; }
		

		//not going to save this ContactPersonName in the database
		[DataMember]
		public string ContactPersonName { get; set; }
		//not going to save this AddressLocation in the database, it will be chopped to street addr, surburb etc
		public string AddressLocation { get { return ToAddressLocation(); } set { LoadAddressInfo(value); } }

		[DataMember]
		public Guid DefaultLocationID { get; set; }
		[DataMember]
		//this is for contact person to store its parent contact's id
		public Guid ParentID { get; set; }

		//& gs-103 more fields needed
		[DataMember]
		public bool IsPerson { get; set; }
		[DataMember]
		public bool IsReseller { get; set; }

		[DataMember]
		public int SourceType { get; set; }  //i360 = 0, iPhone = 1, ...
		[DataMember]
		public Guid SourceID { get; set; }
		[DataMember]
		public Guid StateID { get; set; }    //contact, lead, customer, abandonned, ...

		[DataMember]
		public string Code { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string City { get; set; }
		[DataMember]
		public string Postcode { get; set; }

		[DataMember]
		public string PostalAddress { get; set; }
		[DataMember]
		public string PostalCity { get; set; }
		[DataMember]
		public string PostalPostCode { get; set; }
		[DataMember]
		public string PostalState { get; set; }
		[DataMember]
		public string PostalCountry { get; set; }

		[DataMember]
		public string DirectPhone { get; set; }
		[DataMember]
		public string Website { get; set; }
		[DataMember]
		public string SocialNetwork { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public DateTime LastContactDate { get; set; }
		[DataMember]
		public Guid CreatedByID { get; set; }
		[DataMember]
		public Guid LastUpdateByID { get; set; }
		//. gs-103


#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif
		//# gs-103 fields added
		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Code = GetValue<string>(dr, "Code");
			Name = GetValue<string>(dr, "Name");
			FirstName = GetValue<string>(dr, "FirstName");
			LastName = GetValue<string>(dr, "LastName");
			ContactPersonID = GetValue<Guid>(dr, "ContactPersonID");
			StreetAddress = GetValue<string>(dr, "StreetAddress");
			Suburb = GetValue<string>(dr, "Suburb");
			City = GetValue<string>(dr, "City");
			Postcode = GetValue<string>(dr, "Postcode");
			State = GetValue<string>(dr, "State");
			Country = GetValue<string>(dr, "Country");

			PostalAddress = GetValue<string>(dr, "PostalAddress");
			PostalCity = GetValue<string>(dr, "PostalCity");
			PostalPostCode = GetValue<string>(dr, "PostalPostCode");
			PostalState = GetValue<string>(dr, "PostalState");
			PostalCountry = GetValue<string>(dr, "PostalCountry");

			Phone = GetValue<string>(dr, "Phone");
			DirectPhone = GetValue<string>(dr, "DirectPhone");
			Mobile = GetValue<string>(dr, "Mobile");
			Fax = GetValue<string>(dr, "Fax");
			Email = GetValue<string>(dr, "Email");
			Website = GetValue<string>(dr, "Website");
			SocialNetwork = GetValue<string>(dr, "SocialNetwork");

			JobTitle = GetValue<string>(dr, "JobTitle");
			Type = (ContactType)GetValue<byte>(dr, "Type");
			ClientType = GetValue<byte>(dr, "ClientType");
			Description = GetValue<string>(dr, "Description");

			IsPerson = GetValue<bool>(dr, "IsPerson");
			IsReseller = GetValue<bool>(dr, "IsReseller");

			SourceType = GetValue<int>(dr, "SourceType");
			SourceID = GetValue<Guid>(dr, "SourceID");
			StateID = GetValue<Guid>(dr, "StateID");

			LastContactDate = GetValue<DateTime>(dr, "LastContactDate");
			CreatedByID = GetValue<Guid>(dr, "CreatedByID");
			LastUpdateByID = GetValue<Guid>(dr, "LastUpdateByID");

			DefaultLocationID = GetValue<Guid>(dr, "DefaultLocationID");

			if (HasColumn(dr, "DefaultLocation"))
				DefaultLocation = GetValue<string>(dr, "DefaultLocation");
			if (HasColumn(dr, "ContactPersonName"))
				ContactPersonName = GetValue<string>(dr, "ContactPersonName");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}
		//. gs-103



		/// <summary>
		/// load the street addr, surburb, state, country info from the location
		/// </summary>
		/// <param name="addressLocation"></param>
		public void LoadAddressInfo(string addressLocation)
		{
			LoadAddressInfo(addressLocation, null);
		}
		/// <summary>
		///  load the street addr, surburb, state, country info from the location
		/// </summary>
		/// <param name="addressLocation"></param>
		/// <param name="culture"></param>
		public void LoadAddressInfo(string addressLocation, System.Globalization.CultureInfo culture)
		{
			if (culture == null)
			{
				if (!string.IsNullOrEmpty(addressLocation))
				{
					string[] address = addressLocation.Split(',');
					if (address != null)
					{
						for (int i = 0; i < address.Length; i++)
						{
							address[i] = address[i].Trim();
							switch (i)
							{
								case 3: this.Country = address[i]; break;
								case 2: this.State = address[i]; break;
								case 1: this.Suburb = address[i]; break;
								case 0: this.StreetAddress = address[i]; break;
								default: break;
							}
						}

					}
				}
			}
			//TODO: implement the addresslocation based on culture
		}

		/// <summary>
		/// create address location based on current address info
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public string ToAddressLocation()
		{
			return ToAddressLocation(null);
		}
		/// <summary>
		/// create address location based on current address info
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public string ToAddressLocation(System.Globalization.CultureInfo culture)
		{

			StringBuilder sb = new StringBuilder();
			if (culture == null)
			{
				if (!string.IsNullOrEmpty(this.StreetAddress))
				{
					sb.Append(this.StreetAddress);
				}
				if (!string.IsNullOrEmpty(this.Suburb))
				{
					sb.Append(','); sb.Append(this.Suburb);
				}
				if (!string.IsNullOrEmpty(this.State))
				{
					sb.Append(','); sb.Append(this.State);
				}
				if (!string.IsNullOrEmpty(this.Country) && !this.Country.ToLower().Equals("xx"))//unknow country default to xx
				{
					sb.Append(','); sb.Append(this.Country);
				}
			}
			//TODO: implement the addresslocation based on culture

			return sb.ToString();
		}
	}
}