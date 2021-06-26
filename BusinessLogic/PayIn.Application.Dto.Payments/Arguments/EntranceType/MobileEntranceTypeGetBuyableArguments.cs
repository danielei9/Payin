using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceTypeGetBuyableArguments : IArgumentsBase
    {
        public int CardId { get; set; }
        public int? EventId { get; set; }
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public MobileEntranceTypeGetBuyableArguments(int cardId, int? eventId, int? paymentConcessionId)
        {
            CardId = cardId;
            EventId = eventId;
			PaymentConcessionId = paymentConcessionId;
        }
        #endregion Constructors
    }
}
