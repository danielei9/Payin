using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketRechargeBalanceArguments : IArgumentsBase
	{
													[Required]	public int CardId		{ get; set; }
		[Display(Name = "resources.ticket.amount")] [Required]	public decimal	Amount	{ get; private set; }
		[Display(Name = "resources.ticket.purse")]	[Required]	public int		PurseId { get; set; }

		#region Constructors
		public TicketRechargeBalanceArguments(int cardId, decimal amount, int purseId)
		{
			Amount = amount;
			CardId = cardId;
			PurseId = purseId;
		}
		#endregion Constructors
	}
}