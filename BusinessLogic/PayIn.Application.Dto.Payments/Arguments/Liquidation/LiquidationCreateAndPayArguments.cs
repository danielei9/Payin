using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class LiquidationCreateAndPayArguments : IArgumentsBase
    {
        public int ConcessionId { get; set; }

        #region Constructors
        public LiquidationCreateAndPayArguments(int concessionId)
        {
            ConcessionId = concessionId;
        }
        #endregion Constructors
    }
}
