using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class LiquidationCloseArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructures
        public LiquidationCloseArguments(int id)
            : base()
        {
            Id = id;
        }
        #endregion Constructures
    }
}
