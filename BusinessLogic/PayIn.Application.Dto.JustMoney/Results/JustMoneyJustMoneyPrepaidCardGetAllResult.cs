using PayIn.Domain.JustMoney.Enums;
using PayIn.Infrastructure.JustMoney.Enums;
using Xp.Common;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyJustMoneyPrepaidCardGetAllResult
	{
		// Card
		public int Id { get; set; }
		public decimal Balance { get; set; }
		public string Pan { get; set; }
		public int EspirationCodeYear { get; set; }
		public int EspirationCodeMonth { get; set; }
		public CardStatus CardStatus { get; set; }
        public string CardName { get; set; }
        // Account
        public string Login { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public XpDate BirthDay { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string Country { get; set; }
		public string Mobile { get; set; }
		public string Phone { get; set; }
	}
}
