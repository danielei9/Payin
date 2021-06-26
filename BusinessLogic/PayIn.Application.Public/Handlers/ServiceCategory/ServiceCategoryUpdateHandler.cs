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
	public class ServiceCategoryUpdateHandler : IServiceBaseHandler<ServiceCategoryUpdateArguments>
	{
		[Dependency] public IEntityRepository<ServiceCategory> Repository { get; set; }

		#region Constructors
		public ServiceCategoryUpdateHandler(IEntityRepository<ServiceCategory> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceCategoryUpdateArguments>.ExecuteAsync(ServiceCategoryUpdateArguments arguments)
		{
			var itemServiceCategory = await Repository.GetAsync(arguments.Id);
			itemServiceCategory.Name = arguments.Name ?? "";
			itemServiceCategory.AllMembersInSomeGroup = arguments.AllMembersInSomeGroup;
			itemServiceCategory.AMemberInOnlyOneGroup = arguments.AMemberInOnlyOneGroup;
			itemServiceCategory.AskWhenEmit = arguments.AskWhenEmit;
			itemServiceCategory.DefaultGroupWhenEmitId = arguments.DefaultGroupWhenEmitId;
			return itemServiceCategory;
		}
		#endregion ExecuteAsync

	}
}
