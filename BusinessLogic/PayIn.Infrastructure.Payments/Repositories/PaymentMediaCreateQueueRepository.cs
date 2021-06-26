using PayIn.Application.Dto.Arguments.PaymentMedia;
using Xp.Infrastructure.Azure;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class PaymentMediaCreateQueueRepository : AzureQueueRepository<PaymentMediaCreateArguments>
	{
		#region Contructors
		public PaymentMediaCreateQueueRepository()
			: base("TransferStorageConnectionString", "paymentmedia-create")
		{
		}
		#endregion Contructors
	}
}
