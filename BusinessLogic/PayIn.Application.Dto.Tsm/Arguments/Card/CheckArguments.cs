using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Tsm.Arguments.Card
{
	public class CheckArguments : IArgumentsBase
	{
		[Required] public long Uid { get; set; }
		[Required] public CardType[] CardTypes { get; set; }
	}
}
