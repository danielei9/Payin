using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceCardInquiryResult_CardHolder
	{
		public string FirstName { get; set; }
		public string MiddleInitial { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public JustMoneyCountryEnum CountryCode { get; set; }
		public string Phone { get; set; }
		public DateTime Dob { get; set; }
		//<SSN />
		//<SecurityField1 />
		//<SecurityField2 />
		//<UserDefinedField1 />
		public CardType CardType { get; set; } // UserDefinedField2
		//<UserDefinedField3 />
		//<UserDefinedField4 />
		//<EmailAddr />
		//<CardHolderID>400000625514</CardHolderID>
		public string CardNumber { get; set; }
		//<Address3 />
		//<Address4 />
		//<CountryName />
		//<CountyName />
		//<EmbossName />
		//<SecurityField3 />
		//<SecurityField4 />
		//<SecondaryAddress1 />
		//<SecondaryAddress2 />
		//<SecondaryAddress3 />
		//<SecondaryAddress4 />
		//<City2 />
		//<State2 />
		//<Zip2 />
		//<CountryCode2 />
		//<CountryName2 />
		//<CountyName2 />
		public string Phone2 { get; set; }
		//<DocumentType />
		//<DocumentNumber />
		//<DocumentExpiryDate />
		//<Nationality />
		//<CountryOfIssuance />
		//<udfs>
		//<UDFData>
		//<Name>CompanyName</Name>
		//<Value>PFS</Value>
		//</UDFData>
		//</udfs>

		#region Constructors
		public PfsServiceCardInquiryResult_CardHolder(XElement element)
		{
			FirstName = element
				.Elements()
				.Where(x => x.Name == "FirstName")
				.Select(x => x.Value)
				.FirstOrDefault();
			MiddleInitial = element
				.Elements()
				.Where(x => x.Name == "MiddleInitial")
				.Select(x => x.Value)
				.FirstOrDefault();
			LastName = element
				.Elements()
				.Where(x => x.Name == "LastName")
				.Select(x => x.Value)
				.FirstOrDefault();
			Address1 = element
				.Elements()
				.Where(x => x.Name == "Address1")
				.Select(x => x.Value)
				.FirstOrDefault();
			Address2 = element
				.Elements()
				.Where(x => x.Name == "Address2")
				.Select(x => x.Value)
				.FirstOrDefault();
			City = element
				.Elements()
				.Where(x => x.Name == "City")
				.Select(x => x.Value)
				.FirstOrDefault();
			State = element
				.Elements()
				.Where(x => x.Name == "State")
				.Select(x => x.Value)
				.FirstOrDefault();
			ZipCode = element
				.Elements()
				.Where(x => x.Name == "Zip")
				.Select(x => x.Value)
				.FirstOrDefault();
			CountryCode = element
				.Elements()
				.Where(x => x.Name == "CountryCode")
				.Select(x => x.Value.GetCountryEnum())
				.FirstOrDefault();
			Phone = element
				.Elements()
				.Where(x => x.Name == "Phone")
				.Select(x => x.Value)
				.FirstOrDefault();
			Dob = element
				.Elements()
				.Where(x => x.Name == "DOB")
				.Select(x => new DateTime(int.Parse(x.Value.Substring(4,4)), int.Parse(x.Value.Substring(2,2)), int.Parse(x.Value.Substring(0,2))))
				.FirstOrDefault();
			CardNumber = element
				.Elements()
				.Where(x => x.Name == "CardNumber")
				.Select(x => x.Value)
				.FirstOrDefault();
			CardType = element
				.Elements()
				.Where(x => x.Name == "UserDefinedField2")
				.Select(x => x.Value)
				.FirstOrDefault() == "1" ? CardType.Standard : CardType.Premium;
			Phone2 = element
				.Elements()
				.Where(x => x.Name == "Phone2")
				.Select(x => x.Value)
				.FirstOrDefault();
		}
		#endregion Constructors
	}
}
