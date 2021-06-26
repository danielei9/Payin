using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal.Repositories
{
	public class PaymentMediaRepository : InternalRepository<PaymentMedia>
	{
		#region Contructors
		public PaymentMediaRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
