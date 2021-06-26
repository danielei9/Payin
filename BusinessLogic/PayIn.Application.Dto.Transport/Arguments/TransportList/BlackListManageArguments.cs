using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportList
{
	public class BlackListManageArguments : IArgumentsBase
	{
		[Required]
		public CardType CardType { get; set; }
		[Required]
		public string CardId { get; set; }
		public DateTime ParameterValue { get; set; }
		public MifareOperationResultArguments[] Script { get; set; }
		

		#region Constructors
		public BlackListManageArguments(CardType cardType, string cardId, MifareOperationResultArguments[] script, DateTime parameterValue)
		{
			CardType = cardType;
			CardId = cardId;
			Script = script;
			ParameterValue = parameterValue;
		}
		#endregion Constructors
	}
}
