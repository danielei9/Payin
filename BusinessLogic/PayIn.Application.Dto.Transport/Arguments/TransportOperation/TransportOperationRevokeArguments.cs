using PayIn.Common.Resources;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationRevokeArguments : IArgumentsBase
	{
		[Required]		public CardType CardType { get; set; }
		[Required]		public string CardId { get; set; }
                        public CardSystem CardSystem { get; set; }
						public long? CardNumber { get; set; }
						public MifareOperationResultArguments[] Script { get; set; }
						public RechargeType RechargeType { get; set; }
						public string Imei { get; set; }
						public int? OperationId { get; set; }
						public dynamic Device { get; set; }
						public DateTime Now { get; set; }

		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "PinErrorMessage")]
		[Required(AllowEmptyStrings = false)]
		public string Pin { get; set; }

		#region Constructors
		public TransportOperationRevokeArguments(
			CardType cardType,
			string cardId,
            CardSystem? cardSystem,
            MifareOperationResultArguments[] script,
			RechargeType rechargeType,
			string imei,
			int operationId,
			string pin,
			dynamic device
#if DEBUG || TEST || HOMO
			, DateTime? now
#endif
		)
		{
			CardType = cardType;
			CardId = cardId;
            CardSystem = cardSystem ?? CardSystem.Mobilis;
            CardNumber = cardId.FromHexadecimal().ToInt32();
			Script = script;
			RechargeType = rechargeType;
			Imei = imei;
			OperationId = operationId;
			Pin = pin ?? "1234";
			Device = device;
#if DEBUG || TEST || HOMO
			Now = now ?? DateTime.Now;
#else
			Now = DateTime.Now;
#endif
		}
		#endregion Constructors
	}
}
