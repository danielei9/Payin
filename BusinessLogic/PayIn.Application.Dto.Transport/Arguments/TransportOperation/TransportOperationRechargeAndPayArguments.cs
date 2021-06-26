using PayIn.Common.Resources;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationRechargeAndPayArguments : TransportOperationRechargeArguments
	{
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = true)] public string Pin { get; set; }
		[Required]                           public IEnumerable<TransportOperationRechargeAndPayArguments_PaymentMedia> PaymentMedias { get; set; }
	
		#region Constructors
		public TransportOperationRechargeAndPayArguments(
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
			IEnumerable<TransportOperationRechargeAndPayArguments_PaymentMedia> paymentMedias,
			string deviceManufacturer,
			string deviceModel,
			string deviceName,
			string deviceSerial,
			string deviceId,
			string deviceOperator,
			string deviceImei,
			string deviceMac,
			string operatorSim,
			string operatorMobile,
			string pin
#if DEBUG || TEST || HOMO
			, DateTime? now
#endif
		)
			: base(cardType, cardId, cardSystem, script, code, quantity, priceId, ticketId, rechargeType, promotions, deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile
#if DEBUG || TEST || HOMO
			,  now
#endif
			)
		{
			PaymentMedias = paymentMedias;			
			Pin = pin;
		}
#endregion Constructors
	}
}
