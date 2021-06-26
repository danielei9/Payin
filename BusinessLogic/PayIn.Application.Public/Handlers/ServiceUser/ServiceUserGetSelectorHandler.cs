using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserGetSelectorHandler :
		IQueryBaseHandler<ServiceUserGetSelectorArguments, ServiceUserGetSelectorResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserGetSelectorResult>> ExecuteAsync(ServiceUserGetSelectorArguments arguments)
		{
			// Cargando concesiones
			var concessionIds = (await RepositoryServiceConcession.GetAsync())
				.Where(x => x.Supplier.Login == SessionData.Login ||
							x.Supplier.Workers
								.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => x.Id)
				.ToList();


			var result = (await Repository.GetAsync())
				.Where(x =>
					(
						x.Name.Contains(arguments.Filter) ||
						x.LastName.Contains(arguments.Filter) ||
						x.Email.Contains(arguments.Filter)
					) &&
					concessionIds.Contains(x.ConcessionId)
				)
				.Select (x => new ServiceUserGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name + " " + x.LastName + " - " + x.Email
				 });

			return new ResultBase<ServiceUserGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
