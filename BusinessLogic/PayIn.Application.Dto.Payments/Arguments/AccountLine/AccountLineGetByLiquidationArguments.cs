using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class AccountLineGetByLiquidationArguments : IArgumentsBase
	{
		public string Filter { get; set; }
        public int LiquidationId { get; set; }

		#region Constructors
		public AccountLineGetByLiquidationArguments(string filter, int liquidationId)
		{
			Filter = filter ?? "";
            LiquidationId = liquidationId;
		}
        #endregion Constructors
    }
}
