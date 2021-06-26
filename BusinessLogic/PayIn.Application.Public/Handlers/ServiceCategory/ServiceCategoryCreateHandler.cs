using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCategoryCreateHandler : IServiceBaseHandler<ServiceCategoryCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceCategory> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCategoryCreateArguments arguments)
		{
			var concessionIds = (await RepositoryServiceConcession.GetAsync())
				.Where(x => x.Supplier.Login == SessionData.Login ||
							x.Supplier.Workers
								.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => x.Id)
				.FirstOrDefault();


			var serviceCategory = new ServiceCategory
			{
				Name = arguments.Name,
				AllMembersInSomeGroup = arguments.AllMembersInSomeGroup,
				AMemberInOnlyOneGroup = arguments.AMemberInOnlyOneGroup,
				AskWhenEmit = arguments.AskWhenEmit,
				DefaultGroupWhenEmitId = arguments.DefaultGroupWhenEmitId,
				ServiceConcessionId = concessionIds
			};
			await Repository.AddAsync(serviceCategory);

			return serviceCategory;
		}
		#endregion ExecuteAsync
	}
}
