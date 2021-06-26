using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
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
	public class ApiSystemCardGetSelectorHandler :
		IQueryBaseHandler<ApiSystemCardGetSelectorArguments, SelectorResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<SystemCard> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<SelectorResult>> ExecuteAsync(ApiSystemCardGetSelectorArguments arguments)
		{
            // Empresas de las soy trabajador
            var myServiceConcessions = (await ServiceConcessionRepository.GetAsync("Supplier"))
                .Where(x =>
					(x.Supplier.Login == SessionData.Login) ||
					(x.Supplier.Workers.Any(y => (y.Login == SessionData.Login)))
                );
			
			var items = (await Repository.GetAsync())
				.Where(x =>
					// Emprea o trabajador
					(myServiceConcessions.Any(y => y.Id == x.ConcessionOwnerId)) ||
					// Miembros
					(x.SystemCardMembers
						.Any(y =>
							(myServiceConcessions.Any(z => z.Supplier.Login == y.Login))
						)
					)
				);
            if (!arguments.Filter.IsNullOrEmpty())
                items = items.Where(x =>
                    x.Name.Contains(arguments.Filter)
                );
			
			var result = items
				.Select(x => new SelectorResult
				{
					Id = x.Id,
					Value = x.Name
				})
				.ToList();

			return new ResultBase<SelectorResult> { Data = result.ToList() };
		}
		#endregion ExecuteAsync
	}
}
