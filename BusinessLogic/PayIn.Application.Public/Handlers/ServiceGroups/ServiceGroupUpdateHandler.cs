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
	public class ServiceGroupUpdateHandler : IServiceBaseHandler<ServiceGroupUpdateArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }


		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceGroupUpdateArguments arguments)
		{
			var itemServiceGroup = await Repository.GetAsync(arguments.Id);

			itemServiceGroup.Name = arguments.Name ?? "";
			return itemServiceGroup;
		}
		#endregion ExecuteAsync
	}
}
