using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionGetAllCommerceHandler :
		IQueryBaseHandler<ServiceConcessionGetAllCommerceArguments, ServiceConcessionGetAllCommerceResult>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;
		private readonly IEntityRepository<PaymentConcession> PaymentRepository;
		public readonly ISessionData SessionData;

		#region Constructors
		public ServiceConcessionGetAllCommerceHandler(
			IEntityRepository<ServiceConcession> repository,
			IEntityRepository<PaymentConcession> paymentRepository,
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (paymentRepository == null) throw new ArgumentNullException("paymentRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			PaymentRepository = paymentRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceConcessionGetAllCommerceResult>> IQueryBaseHandler<ServiceConcessionGetAllCommerceArguments, ServiceConcessionGetAllCommerceResult>.ExecuteAsync(ServiceConcessionGetAllCommerceArguments arguments)
		{
			var now = DateTime.MinValue;
			var items = (await Repository.GetAsync())
				.Where(x => x.Supplier.Login == SessionData.Login);
			
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter) ||
						x.Supplier.Name.Contains(arguments.Filter)
					);

			var paymentConcesion = (await PaymentRepository.GetAsync());

			var result = items
				.Where(y => y.State != Common.ConcessionState.Removed)
				.Select(x => new 
				{
					Id = x.Id,
					Name = x.Name,
					SupplierId = x.SupplierId,
					Type = x.Type,
					State = x.State,
					SupplierName = x.Supplier.Name,
					PaymentConcessionInfo = paymentConcesion.Where(z => z.ConcessionId == x.Id).FirstOrDefault()
				})
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					SupplierId = x.SupplierId,
					Type = x.Type,
					State = x.State,
					SupplierName = x.SupplierName,
					PayinCommision = (decimal) ((decimal?) x.PaymentConcessionInfo.PayinCommision ?? 0m),
					CreateConcessionDate = (DateTime) ((DateTime?) x.PaymentConcessionInfo.CreateConcessionDate ?? now)
				})
				.ToList()
				.Select(x => new ServiceConcessionGetAllCommerceResult
				{
					Id = x.Id,
					Name = x.Name,
					SupplierId = x.SupplierId,
					Type = x.Type,
					TypeName = x.Type.ToEnumAlias(),
					State = x.State,
					SupplierName = x.SupplierName,
					PayinCommision = x.PayinCommision,
					CreateConcessionDate = x.CreateConcessionDate 
				});
			return new ResultBase<ServiceConcessionGetAllCommerceResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
