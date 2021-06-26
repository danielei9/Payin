using PayIn.Infrastructure.JustMoney.Enums;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class PrepaidCardGetAllResult
	{
		public int Id { get; set; }
		public string Pan { get; set; }
		public string Caducidad { get; set; }
		public string NumberIde { get; set; }
		public CardStatus Status { get; set; }
		public string UserName { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
	}
}
