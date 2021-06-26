using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public class TransportOperationRechargeConfirmArguments : IArgumentsBase
	{
		[Required] public CardType CardType { get; set; }
		[Required] public string CardId { get; set; }
		           public long? CardNumber { get; set; }
		           public MifareOperationResultArguments[] Script { get; set; }
		           public int Id { get; set; }//Id transportPrice a recargar.
				   public int IdTicket { get; set; }//Id ticket

		#region Constructors
		public TransportOperationRechargeConfirmArguments(CardType cardType, string cardId, MifareOperationResultArguments[] script,int id,int idTicket)
		{
			CardType = cardType;
			CardId = cardId;
			CardNumber = cardId.FromHexadecimal().ToInt32();
			Script = script;
			Id = id;
			IdTicket = IdTicket;
		}
		#endregion Constructors
	}
}
