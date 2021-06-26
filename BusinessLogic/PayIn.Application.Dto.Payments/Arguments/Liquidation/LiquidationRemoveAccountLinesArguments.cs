using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class LiquidationRemoveAccountLinesArguments : IArgumentsBase
    {
        public int Id { get; set; }
        public int AccountLineId { get; set; }

        #region Constructures
        public LiquidationRemoveAccountLinesArguments(int accountLineId)
			: base()
		{
            AccountLineId = accountLineId;
        }
		#endregion Constructures
	}
}
