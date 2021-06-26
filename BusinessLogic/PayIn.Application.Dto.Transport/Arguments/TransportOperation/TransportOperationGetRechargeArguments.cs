using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public class TransportCardGetRechargeArguments : IArgumentsBase
	{
		[Required]
		public CardType CardType { get; set; }
		[Required]
		public string CardId { get; set; }
		public long? CardNumber { get; set; }

		#region Constructors
		public TransportCardGetRechargeArguments(CardType cardType, string cardId)
		{
			CardType = cardType;
			CardId = cardId;
			CardNumber = cardId.FromHexadecimal().ToInt32();
		}
		#endregion Constructors
	}
}
