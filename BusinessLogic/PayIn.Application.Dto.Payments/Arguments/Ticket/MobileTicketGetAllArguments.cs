using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileTicketGetAllArguments : IArgumentsBase
	{
        public int PaymentConcessionId { get; set; }

        #region Constructors
        public MobileTicketGetAllArguments(int paymentConcessionId)
        {
            PaymentConcessionId = paymentConcessionId;
        }
        #endregion Constructors
    }
}
