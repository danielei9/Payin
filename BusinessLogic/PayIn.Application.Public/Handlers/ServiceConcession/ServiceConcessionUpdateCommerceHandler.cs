using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionUpdateCommerceHandler :
		IServiceBaseHandler<ServiceConcessionUpdateCommerceArguments>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;

		#region Constructors
		public ServiceConcessionUpdateCommerceHandler(IEntityRepository<ServiceConcession> repository, IEntityRepository<PaymentConcession> paymentConcessionRepository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");

			Repository = repository;
			PaymentConcessionRepository = paymentConcessionRepository;

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceConcessionUpdateCommerceArguments>.ExecuteAsync(ServiceConcessionUpdateCommerceArguments arguments)
		{
			var result = await Repository.GetAsync(arguments.Id);
			
			if(result.State == ConcessionState.Pending)
			{
				result.Name = arguments.Name;
			}
			return result;
		}
		#endregion ExecuteAsync
	}
}
