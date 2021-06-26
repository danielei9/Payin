using PayIn.Domain.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Domain;

namespace PayIn.Domain.JustMoney
{
	public class PrepaidCard : Entity
	{
		                                      public PrepaidCardState State { get; set; }
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }
		[Required(AllowEmptyStrings = false)] public string CardHolderId { get; set; }
		[Required(AllowEmptyStrings = false)] public string Alias { get; set; }
		[Required(AllowEmptyStrings = false)] public string Pan { get; set; }
	}
}
