using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
namespace PayIn.Application.Public
{
	public class ServiceTagCreateHandler :
		IServiceBaseHandler<ServiceTagCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceTag> Repository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceTagCreateArguments>.ExecuteAsync(ServiceTagCreateArguments arguments)
		{
			var itemServiceTag = new ServiceTag
			{
				Type = arguments.Type,
				SupplierId = arguments.SupplierId,
				Reference = arguments.Reference,

			};
			await Repository.AddAsync(itemServiceTag);
			return itemServiceTag;
		}
		#endregion ExecuteAsync
	}
}