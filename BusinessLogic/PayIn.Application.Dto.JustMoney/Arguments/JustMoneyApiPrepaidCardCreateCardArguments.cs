using System.ComponentModel.DataAnnotations;
using PayIn.Infrastructure.JustMoney.Enums;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardCreateCardArguments : IArgumentsBase
	{
		public int CardId { get; set; }
		public string Alias1 { get; set; }
		public string Alias2 { get; set; }
		public string Alias3 { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string ZipCode { get; set; }
		[Required] public JustMoneyCountryEnum Country { get; set; }
		
		#region Constructors
		public JustMoneyApiPrepaidCardCreateCardArguments(int cardId, string alias1, string alias2, string alias3, string address1, string address2, string city, string province, string zipCode, JustMoneyCountryEnum country)
        {
			CardId = cardId;
			Alias1 = alias1;
			Alias2 = alias2;
			Alias3 = alias3;
			Address1 = address1;
			Address2 = address2;
			City = city;
			Province = province;
			ZipCode = zipCode;
			Country = country;
		}
		#endregion Constructors
	}
}
