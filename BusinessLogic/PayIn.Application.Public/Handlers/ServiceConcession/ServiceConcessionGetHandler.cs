using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionGetHandler :
		IQueryBaseHandler<ServiceConcessionGetArguments, ServiceConcessionGetResult>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<ServiceUser> ServiceUserRepository;

		#region Constructors
		public ServiceConcessionGetHandler(
			IEntityRepository<ServiceConcession> repository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			IEntityRepository<ServiceUser> serviceUserRepository
		)
		{
			if (repository == null) throw new ArgumentNullException(nameof(repository));
			if (paymentConcessionRepository == null) throw new ArgumentNullException(nameof(paymentConcessionRepository));
			if (serviceUserRepository == null) throw new ArgumentNullException(nameof(serviceUserRepository));

			Repository = repository;
			PaymentConcessionRepository = paymentConcessionRepository;
			ServiceUserRepository = serviceUserRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceConcessionGetResult>> ExecuteAsync(ServiceConcessionGetArguments arguments)
		{
			var paymentConcesions = (await PaymentConcessionRepository.GetAsync())
				.Where(x => x.ConcessionId == arguments.Id);

			var users = (await ServiceUserRepository.GetAsync());

			var result = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ServiceConcessionGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					MaxWorkers = x.MaxWorkers,
					State = x.State,
					PhotoUrl = users
						.Where(y => y.Login == x.Supplier.Login)
						.Select(y => y.Photo)
						.FirstOrDefault(),
					PayinComission = paymentConcesions.Any() && x.Type == ServiceType.Charge ?
						paymentConcesions
							.Sum(y => y.PayinCommision) :
						0m,
					LiquidationAmountMin = paymentConcesions.Any() && x.Type == ServiceType.Charge ?
						paymentConcesions
							.Sum(y => y.LiquidationAmountMin) :
						0m,
				});

			return new ResultBase<ServiceConcessionGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
