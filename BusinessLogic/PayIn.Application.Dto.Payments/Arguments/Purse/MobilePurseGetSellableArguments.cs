using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobilePurseGetSellableArguments : IArgumentsBase
    {
		public int ServiceCardId { get; set; }

		#region Constructors
		public MobilePurseGetSellableArguments(int serviceCardId)
		{
            ServiceCardId = serviceCardId;
		}
        #endregion Constructors
    }
}
