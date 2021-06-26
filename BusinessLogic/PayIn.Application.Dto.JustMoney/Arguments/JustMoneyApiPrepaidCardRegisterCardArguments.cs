using System.ComponentModel.DataAnnotations;
using PayIn.Infrastructure.JustMoney.Enums;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardRegisterCardArguments : IArgumentsBase
	{
		public int CardId { get; set; }
		public string CardHolderId { get; set; }
		public string Alias { get; set; }
		//public string Address1 { get; set; }
		//public string Address2 { get; set; }
		//public string City { get; set; }
		//public string Province { get; set; }
		//public string ZipCode { get; set; }
		//[Required] public JustMoneyCountryEnum Country { get; set; }
		
		#region Constructors
		public JustMoneyApiPrepaidCardRegisterCardArguments(
			int cardId, string cardHolderId,
			string alias
			//string address1, string address2, string city, string province, string zipCode, JustMoneyCountryEnum country
		)
        {
			CardId = cardId;
			CardHolderId = cardHolderId;
			Alias = alias;
			//Address1 = address1;
			//Address2 = address2;
			//City = city;
			//Province = province;
			//ZipCode = zipCode;
			//Country = country;
		}
		#endregion Constructors
	}
}
