using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobilePurseGetBuyableArguments : IArgumentsBase
    {
		public int ServiceCardId { get; set; }

		#region Constructors
		public MobilePurseGetBuyableArguments(int serviceCardId)
		{
            ServiceCardId = serviceCardId;
		}
        #endregion Constructors
    }
}
