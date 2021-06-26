using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketDonateBalanceArguments : IArgumentsBase
	{
													[Required]	public int CardId { get; set; }

		//[Display(Name = "resources.ticket.uid")]				public long		Uid		{ get; set; }
		//														public int PaymentConcessionId { get; set; }
		[Display(Name = "resources.ticket.amount")] [Required]	public decimal	Amount	{ get; private set; }
		[Display(Name = "resources.ticket.purse")]	[Required]	public int		PurseId { get; set; }
		//[Display(Name = "resources.ticket.purse")]	[Required]	public int		Id		{ get; set; }
		//[Display(Name = "resources.ticket.purse")]	[Required]	public string	UidText { get; set; }

		#region Constructors
		//public TicketDonateBalanceArguments(decimal amount, int id, int purseId, long uid, string uidText, int paymentConcessionId)
		public TicketDonateBalanceArguments(int cardId, decimal amount, int purseId)
		{
			CardId = cardId;
			Amount = amount;
			//Id = id;
			PurseId = purseId;
			//Uid = uid;
			//UidText = uidText;
			//PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}