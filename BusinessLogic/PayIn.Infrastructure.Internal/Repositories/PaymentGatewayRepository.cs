using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal.Queues
{
	public class PaymentGatewayRepository : InternalRepository<PaymentGateway>
	{
		#region Contructors
		public PaymentGatewayRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
