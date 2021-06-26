using PayIn.Infrastructure.JustMoney.Enums;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyApiPrepaidCardGetAllResult
	{
        public int Id { get; set; }
        public string CardHolderId { get; set; }
		public string Alias { get; set; }

		public string AvailableBalance { get; set; }
		public string Pan { get; set; }
		public int EspirationCodeYear { get; set; }
		public int EspirationCodeMonth { get; set; }
		public CardStatus CardStatus { get; set; }
		public CardType CardType { get; set; }
		public string Iban { get; set; }
		
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public string Mobile { get; set; }
		public string Phone { get; set; }
	}
}
