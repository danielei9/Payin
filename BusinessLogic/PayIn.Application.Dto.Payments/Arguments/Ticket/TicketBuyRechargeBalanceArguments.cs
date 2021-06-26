using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketBuyRechargeBalanceArguments : IArgumentsBase
	{
																public int      ServiceCardId		{ get; set; }
		[Display(Name = "resources.ticket.amount")] [Required]	public decimal	Amount	            { get; private set; }
		[Display(Name = "resources.ticket.purse")]	[Required]	public int		PurseId             { get; set; }

		#region Constructors
		public TicketBuyRechargeBalanceArguments(int serviceCardId, decimal amount, int purseId)
		{
			ServiceCardId = serviceCardId;
			Amount = amount;
			PurseId = purseId;
		}
		#endregion Constructors
	}
}