using PayIn.Domain.JustMoney.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.JustMoney
{
	public class BankCardTransaction : Entity
	{
		                                      public DateTime CreatedAt { get; set; }
		                                      public PrepaidCardState State { get; set; }
		[Required(AllowEmptyStrings = true)]  public string Login { get; set; } = "";
		[Required(AllowEmptyStrings = false)] public string RegistrationMessageId { get; set; } = "";
		[Required(AllowEmptyStrings = false)] public string CardHolderId { get; set; } = "";
		                                      public decimal Amount { get; set; }

		[Required(AllowEmptyStrings = true)]  public string Token { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Pan { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string Type { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string ExpirationMM { get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string ExpirationYYYY { get; set; } = "";

		[Required(AllowEmptyStrings = true)]  public string DepositReferenceId { get; set; } = "";
	}
}
