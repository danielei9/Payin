using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceTypeGetToGiveArguments : IArgumentsBase
    {
		public int CardId { get; set; }
		public int? EventId { get; set; }
		public int? SystemCardId { get; set; }
		//public long? Uid { get; set; }
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public MobileEntranceTypeGetToGiveArguments(int cardId, int? eventId, int? systemCardId, int? paymentConcessionId)
        {
			CardId = cardId;
			EventId = eventId;
            SystemCardId = systemCardId;
            //Uid = uid;
			PaymentConcessionId = paymentConcessionId;
		}
        #endregion Constructors
    }
}
