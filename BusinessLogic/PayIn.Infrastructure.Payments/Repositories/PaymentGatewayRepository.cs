using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentGatewayRepository : PublicRepository<Gateway>
	{
		#region Contructors
		public PaymentGatewayRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
