using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentGetAllByLiquidationArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int? LiquidationId { get; set; }
        public int ConcessionId { get; set; }

        #region Constructors
        public PaymentGetAllByLiquidationArguments(string filter, int? liquidationId, int concessionId)
		{
			Filter = filter ?? "";

			LiquidationId = liquidationId;
            ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}
