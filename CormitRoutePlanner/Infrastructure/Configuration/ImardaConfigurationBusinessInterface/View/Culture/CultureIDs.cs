using System;
using System.Collections.Generic;
using System.Text;

namespace ImardaConfigurationBusiness
{
	/// <summary>
	/// Contains Culture IDs.
	/// </summary>
	public class CultureIDs
	{
		#region CultureIDs is a Singleton

		public static CultureIDs Instance
		{
			get
			{
				if (_Instance == null) _Instance = new CultureIDs();
				return _Instance;
			}
		}

		private static CultureIDs _Instance;

		/// <summary>
		/// Constructs the one and only CultureIDs.
		/// </summary>
		private CultureIDs()
		{
			_Map = new Dictionary<string, Guid>();
			int n = _Data.GetLength(0);
			for (int i = 0; i < n; i++)
			{
				_Map[_Data[i, 0]] = new Guid(_Data[i, 1]);
			}
		}

		#endregion


		private Dictionary<string, Guid> _Map;

		public Guid GetCountryGuid(string isoTwoLetterCountryCode)
		{
			Guid value;
			return _Map.TryGetValue(isoTwoLetterCountryCode, out value) ? value : Guid.Empty;
		}



		private static string[,] _Data = new string[,]
		{
			{"AE", "63249017-2397-4623-8f13-e1a75aab2c2a"}, // United Arab Emirates
			{"AL", "b34a7aa7-3af8-4f35-9c25-fd47d5977840"}, // Albania
			{"AM", "d2221ebc-a090-4d29-a333-0dde7afc6f10"}, // Armenia
			{"AR", "5386c527-1b8a-468a-b313-2b66edf8b872"}, // Argentina
			{"AT", "e863b17d-e8c0-4fcc-8000-dad5df956f90"}, // Austria
			{"AU", "cec49a42-97c7-4d86-b65e-2d8599752b1c"}, // Australia
			{"AZ", "81f89812-fa04-46ce-8937-8b1f894c221b"}, // Azerbaijan
			{"BE", "79d25e1c-8cff-4df1-868f-61f9ed72e67e"}, // Belgium
			{"BG", "91d1363d-e268-480e-82e0-99fe7aceb6b9"}, // Bulgaria
			{"BH", "9673d815-a1fa-4dec-9065-a39e9d659d9a"}, // Bahrain
			{"BN", "4ac1e997-653e-42f9-8210-c9d02a9bb5e3"}, // Brunei
			{"BO", "53d3fcce-ac01-4cd6-9978-1460dd7440df"}, // Bolivia
			{"BR", "20156120-d6c7-4db3-9084-2a022569e092"}, // Brazil
			{"BY", "0dca1b7a-2256-4fac-8baf-7ee74ca46d14"}, // Belarus
			{"BZ", "849a4ef6-0f66-4e8d-a52e-d3f257cd5195"}, // Belize
			{"CA", "c254d2c6-cacb-4455-89ad-f93d5865512a"}, // Canada
			{"CB", "4672d680-422c-40f4-ac96-9f66694df562"}, // Caribbean
			{"CH", "d51db709-6fb4-4d15-9181-474c598bb8c6"}, // Switzerland
			{"CL", "fdbf36cb-9aed-4886-89ac-004faf1d28a9"}, // Chile
			{"CN", "14768d2e-4450-478f-94ba-3348510d9aec"}, // China
			{"CO", "eb13cc4e-3714-4781-9946-be8b74405117"}, // Colombia
			{"CR", "5b64197f-65eb-4d39-88b8-4d8b8a78ab30"}, // Costa Rica
			{"CZ", "8654dc25-daa7-436a-b87a-37a6bb298ae2"}, // Czech Republic
			{"DE", "182d2539-191e-4ec9-a542-d94f93c8c17b"}, // Germany
			{"DK", "a8bb8356-cf41-42a6-bb09-745f892a57f0"}, // Denmark
			{"DO", "14843764-0358-40c4-918f-2b7e7a9a565e"}, // Dominican Republic
			{"DZ", "89b50b9a-7aff-4cfb-94b7-b623f25b8653"}, // Algeria
			{"EC", "1cfd4d1f-ab6e-4d25-8a8a-dea0634594eb"}, // Ecuador
			{"EE", "b26bbd01-fea5-4d85-a25e-33d980d0d232"}, // Estonia
			{"EG", "325b2d0b-63b1-43e4-a852-50e30a476159"}, // Egypt
			{"ES", "e814a971-33c8-4dcb-a213-511eb99c55fb"}, // Spain
			{"FI", "72237615-278b-46e3-8559-52c82febf79a"}, // Finland
			{"FO", "d5236cbb-b5b3-4ffc-a261-61dd28c373d3"}, // Faroe Islands
			{"FR", "e5204aa9-e72a-43b3-9b16-6f051ade3e2d"}, // France
			{"GB", "1a203ad4-2d82-4286-8ab9-62eb29035be7"}, // United Kingdom
			{"GE", "d954b1e1-643d-4ce6-94b5-29859da5a269"}, // Georgia
			{"GR", "c6f2c9ce-d877-40e0-aee5-25effdc8d938"}, // Greece
			{"GT", "50ba4d27-8655-4f7d-b1bc-f24a1e697853"}, // Guatemala
			{"HK", "3da72412-52b2-48af-b143-318897cfc01d"}, // Hong Kong SAR
			{"HN", "0e9e2e01-3de9-4ca3-866e-78a9c312cee5"}, // Honduras
			{"HR", "6f8f2716-1df4-41ed-894f-00fb7fe2a321"}, // Croatia
			{"HU", "a6e3dd73-112f-4edf-b3a1-a428a28add70"}, // Hungary
			{"ID", "b9bad9c8-3be3-477d-abc5-5417b65c3b10"}, // Indonesia
			{"IE", "eae18421-9348-4c7a-9ee5-a9b3815eac34"}, // Ireland
			{"IL", "9805e799-40a1-4906-b4a1-efd0d47960da"}, // Israel
			{"IN", "22d07710-80ad-405e-b491-de9f76ec1c1d"}, // India
			{"IQ", "c4ba82e8-158a-4f2e-8247-632e26d0fa7e"}, // Iraq
			{"IR", "1195543e-3a38-41bd-b364-4ec40a089993"}, // Iran
			{"IS", "d9a2ffcc-5b8b-464b-9131-87e37fd25821"}, // Iceland
			{"IT", "581d7c8b-1cbd-42ef-9a68-b21fa381195e"}, // Italy
			{"JM", "7c05af7c-016b-4e78-a63a-ee4e31909908"}, // Jamaica
			{"JO", "01fed127-a975-49d5-832f-c50cca061234"}, // Jordan
			{"JP", "b2d5bce3-2076-4611-9e00-eec6258644ba"}, // Japan
			{"KE", "6a36815a-8edc-4592-96b5-2711b778536b"}, // Kenya
			{"KG", "7faead5c-d954-4b14-86fd-1db3c4f5b2b0"}, // Kyrgyzstan
			{"KR", "775bdab5-b96a-4e28-9493-c75c93cf5d99"}, // Korea
			{"KW", "9ab8ac23-e07e-4f25-9feb-076f8cae0c11"}, // Kuwait
			{"KZ", "5cd602f0-e4a8-423a-ad5e-0d8c812feb27"}, // Kazakhstan
			{"LB", "4e6eb340-26b3-4359-ab2a-b98803ec2518"}, // Lebanon
			{"LI", "5737af3f-4ffc-4cc7-bd1a-06f3089e9578"}, // Liechtenstein
			{"LT", "fd665d7d-20ee-4ba1-9b69-524b5533f32b"}, // Lithuania
			{"LU", "56d5d1a5-9970-4664-ade4-70a01b68c47a"}, // Luxembourg
			{"LV", "2e6b14ac-dd13-4479-8b0f-18d13c477c8d"}, // Latvia
			{"LY", "6dca938d-0250-4bb6-b0a8-cd6c21ecf54d"}, // Libya
			{"MA", "f0578739-25d3-46a1-8066-8bac58868cab"}, // Morocco
			{"MC", "e7e84240-23b5-4af9-87d5-a2202ea5bc0d"}, // Monaco
			{"MK", "c812e1e7-6b48-423e-9345-0bc77a1136df"}, // Macedonia
			{"MN", "de3dfda1-aef1-443a-bdc2-b6054bda0448"}, // Mongolia
			{"MO", "87681739-a9d9-44f5-8da3-e144d2cab09e"}, // Macao SAR
			{"MV", "ae90bef9-085a-4e17-9677-9a0d9db93103"}, // Maldives
			{"MX", "cec8e4f2-6c97-4739-8199-c1622b3df24d"}, // Mexico
			{"MY", "d6993a29-b24c-4fc1-82e0-44d10b79d768"}, // Malaysia
			{"NI", "a5f45c00-9737-49c2-a2c7-5ee788dc1b72"}, // Nicaragua
			{"NL", "d1f55ba7-b809-4440-b9f8-6f83f0d8be95"}, // The Netherlands
			{"NO", "92cb077b-1297-445c-870c-0c87517ac35c"}, // Norway
			{"NZ", "7eb78b16-84b0-433b-acb7-119bff0a915e"}, // New Zealand
			{"OM", "45000d59-dafc-49cb-b6a2-bd593a9d443a"}, // Oman
			{"PA", "971f8ef8-7074-4a77-8cd9-4bbcd9d84a66"}, // Panama
			{"PE", "03cc2aa7-f1f7-421a-89a3-48f8a8a04458"}, // Peru
			{"PH", "eb207c4f-4e6c-483f-bb74-8d75a87d7812"}, // Philippines
			{"PK", "304a7c56-543e-404d-acb1-41cacf5ae97d"}, // Pakistan
			{"PL", "b3b953e1-15dd-4482-b403-3e79f3756fdc"}, // Poland
			{"PR", "bc395683-508d-4c1f-9348-a6b4bf865e14"}, // Puerto Rico
			{"PT", "a2d9ddab-4a12-4687-ba98-00370e96f71e"}, // Portugal
			{"PY", "8dc07486-d469-4a6d-858b-2fdc842cebf9"}, // Paraguay
			{"QA", "86f7906c-16a3-4a2a-9a6d-4eb665ce48bf"}, // Qatar
			{"RO", "2c073c05-881e-41a2-b003-fd6a437ae92d"}, // Romania
			{"RU", "cf3f0deb-a726-44ed-8357-854462ea12cd"}, // Russia
			{"SA", "1303378f-56b1-4bb3-9550-89e1c9ee92e9"}, // Saudi Arabia
			{"SE", "2c885a50-7c87-4591-9dcc-326e1630cf9c"}, // Sweden
			{"SG", "c0bd72f5-697b-4df7-815e-484bdfa62018"}, // Singapore
			{"SI", "1e99006b-5c97-4d1a-b9b8-0844605cae15"}, // Slovenia
			{"SK", "62be8b4d-4e88-4308-9dc3-69061194057f"}, // Slovakia
			{"SP", "05bf654d-ba3f-46c9-8622-3cde6f49ab76"}, // Serbia
			{"SV", "e3dd5592-193d-4e30-96da-364fcdecf039"}, // El Salvador
			{"SY", "50866cf3-199e-40c4-a41d-8535b2e81fe1"}, // Syria
			{"TH", "4efc586f-1918-46c7-9035-d885d970d5a8"}, // Thailand
			{"TN", "71eb4ada-3f02-4b42-8e67-453a770473ee"}, // Tunisia
			{"TR", "dbe89046-08bc-45e9-ad16-1cc7bb4faa40"}, // Turkey
			{"TT", "78058150-ad22-4140-a57b-9d7a8d115bc2"}, // Trinidad and Tobago
			{"TW", "60a67a9b-a6ed-4b92-8ad5-02e4ddab597e"}, // Taiwan
			{"UA", "40119e31-ecdf-4813-a92a-1ab052fad946"}, // Ukraine
			{"US", "f205f2c2-52bc-49d2-ba66-148bb9f03d65"}, // United States
			{"UY", "e26d84a3-da85-4394-8ee8-7bda0cebd9ac"}, // Uruguay
			{"UZ", "e441ea63-c528-40d2-9150-5027818612c2"}, // Uzbekistan
			{"VE", "284fe80b-665b-4d68-bfe7-424085574701"}, // Venezuela
			{"VN", "cc0ffe54-7298-4325-ac63-7275201603ea"}, // Vietnam
			{"YE", "9c82b15a-1b66-48f0-b758-6a0c67a1db4d"}, // Yemen
			{"ZA", "6e102cc8-6cb5-41d0-be88-96d31d3d3224"}, // South Africa
			{"ZW", "ee39b4b0-f079-45ed-b3a4-45dbc28f70a7"}, // Zimbabwe
		};
	}
}
