using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionUpdateCommerceHandler :
		IServiceBaseHandler<PaymentConcessionUpdateCommerceArguments>
	{
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<ServiceConcession> ConcessionRepository;

		#region Constructors
		public PaymentConcessionUpdateCommerceHandler(IEntityRepository<ServiceConcession> repository, IEntityRepository<PaymentConcession> paymentConcessionRepository, IEntityRepository<ServiceConcession> concessionRepository)
		{
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
			if (concessionRepository == null) throw new ArgumentNullException("concessionRepository");

			PaymentConcessionRepository = paymentConcessionRepository;
			ConcessionRepository = concessionRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentConcessionUpdateCommerceArguments>.ExecuteAsync(PaymentConcessionUpdateCommerceArguments arguments)
		{
			var item = (await PaymentConcessionRepository.GetAsync(arguments.Id));

			item.BankAccountNumber = arguments.BankAccountNumber;
			item.LiquidationAmountMin = arguments.LiquidationAmountMin;
			item.Phone = arguments.Phone;
			item.Address = arguments.Address;
			item.Observations = arguments.Observations;
			
			if (arguments.FormA != null)
			{
				var repositoryAzure = new AzureBlobRepository();
				var guid = Guid.NewGuid();

				try
				{
					if (!item.FormUrl.IsNullOrEmpty())
						repositoryAzure.DeleteFile(item.FormUrl);
				}
				catch { }
				
				item.FormUrl = repositoryAzure.SaveFile(PaymentConcessionResources.FileShortUrl.FormatString(item.Id, guid), arguments.FormA);
			}			
			return item;
		}
		#endregion ExecuteAsync
	}
}