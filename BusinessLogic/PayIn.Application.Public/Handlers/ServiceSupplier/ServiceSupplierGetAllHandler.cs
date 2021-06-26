using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Application.Dto.Results.ServiceSupplier;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceSupplierGetAllHandler :
		IQueryBaseHandler<ServiceSupplierGetAllArguments, ServiceSupplierGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceSupplier> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceSupplierGetAllResult>> ExecuteAsync(ServiceSupplierGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();
			var paymentConcession = await PaymentRepository.GetAsync();

			var result = items
				.Where(x => x.Name.Contains(arguments.Filter) || x.Concessions.Any(z => z.Name.Contains(arguments.Filter)))
				.Select(x => new ServiceSupplierGetAllResult
				{
					Id   = x.Id,
					Name = x.Name,
					Concessions = x.Concessions
					.Where(y => x.Name.Contains(arguments.Filter) || y.Name.Contains(arguments.Filter))
					.Select(y => new ServiceSupplierGetAllResult.Concession 
					{
						Id    = y.Id,
						Name  = y.Name,
						Type  = y.Type,
						State = y.State
					})
				})
				.ToList()
				.Select(x => new ServiceSupplierGetAllResult
				{
					Id   = x.Id,
					Name = x.Name,
					Concessions = x.Concessions.Select(y => new ServiceSupplierGetAllResult.Concession 
					{
						Id        = y.Id,
						Name      = y.Name,
						Type      = y.Type,
						TypeName  = y.Type.ToEnumAlias(),
						MaxWorkers = y.MaxWorkers,
						State     = y.State,
						StateName = y.State.ToEnumAlias(),
						PaymentConcessionId = paymentConcession.Where(z => z.ConcessionId == y.Id).Select(a => a.Id).FirstOrDefault(),
						PayinCommission = paymentConcession.Where(z => z.ConcessionId == y.Id).Select(a => a.PayinCommision).FirstOrDefault(),
						CreateConcessionDate = paymentConcession.Where(w => w.ConcessionId == y.Id).Select(b => b.CreateConcessionDate).FirstOrDefault(),
						FormUrl = paymentConcession.Where(v => v.ConcessionId == y.Id).Select(c => c.FormUrl).FirstOrDefault()
					})
				})				
			.OrderBy(x => x.Name);

			return new ResultBase<ServiceSupplierGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
