using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
    public class ShopByConcessionArguments : IArgumentsBase
    {
        // public string Filter { get; set; }
        public int PaymentConcessionId { get; set; }
		public int? ServiceCardId { get; set; }

		#region Constructors
		public ShopByConcessionArguments(int paymentConcessionId, int? serviceCardId)
        {
			//Filter = filter ?? "";
			PaymentConcessionId = paymentConcessionId;
			ServiceCardId = serviceCardId;
		}
        #endregion Constructors
    }
}
