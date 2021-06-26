using PayIn.Domain.Transport;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationRechargeArguments : MobileConfigurationArguments
	{
		[Required]	   public CardType CardType { get; set; }
		[Required]     public string CardId { get; set; }
                       public CardSystem CardSystem { get; set; }
		               public long? CardNumber { get; set; }
		               public MifareOperationResultArguments[] Script { get; set; }
					   public int Code { get; set; }//Id transportPrice a recargar.
					   public decimal? Quantity { get; set; }
			           public int PriceId { get; set; }
			 		   public int TicketId { get; set; }
					   public RechargeType RechargeType { get; set; }
					   public string Imei { get; set; }
		               public int? OperationId { get; set; }
		               public DateTime Now { get; set; }
		               public dynamic Device { get; set; }
                       public IEnumerable<TransportOperationRechargeArguments_Promotion> Promotions { get; set; }	

		#region Constructors
		public TransportOperationRechargeArguments(
			CardType cardType,
			string cardId,
            CardSystem? cardSystem,
            MifareOperationResultArguments[] script,
			int code,
			decimal? quantity,
			int? priceId,
			int ticketId,
			RechargeType rechargeType,
			IEnumerable<TransportOperationRechargeArguments_Promotion> promotions,
			string deviceManufacturer,
			string deviceModel,
			string deviceName,
			string deviceSerial,
			string deviceId,
			string deviceOperator,
			string deviceImei,
			string deviceMac,
			string operatorSim,
			string operatorMobile
#if DEBUG || TEST || HOMO
			, DateTime? now
#endif
		)
			: base(deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile)
		{
			CardType = cardType;
			CardId = cardId;
            CardSystem = cardSystem ?? CardSystem.Mobilis;
            CardNumber = cardId.FromHexadecimal().ToInt32();
			Script = script;
			Code = code;
			Quantity = quantity;
			PriceId = priceId ?? 0;
			TicketId = ticketId;
			RechargeType = rechargeType;
			Promotions = promotions ?? new List<TransportOperationRechargeArguments_Promotion>();
#if DEBUG || TEST || HOMO
			Now = now ?? DateTime.Now;
#else
			Now = DateTime.Now;
#endif
		}
#endregion Constructors
	}
}
