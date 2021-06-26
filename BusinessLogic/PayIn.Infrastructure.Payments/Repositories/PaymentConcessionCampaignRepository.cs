using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
    public class PaymentConcessionCampaignRepository : PublicRepository<PaymentConcessionCampaign>
	{

		#region Contructors
		public PaymentConcessionCampaignRepository(
			IPublicContext context
		)
			: base(context)
		{
			
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PaymentConcessionCampaign> CheckHorizontalVisibility(IQueryable<PaymentConcessionCampaign> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
