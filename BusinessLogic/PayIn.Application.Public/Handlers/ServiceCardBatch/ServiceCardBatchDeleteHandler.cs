using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardBatchDeleteHandler : IServiceBaseHandler<ServiceCardBatchDeleteArguments>
	{
		[Dependency] public IEntityRepository<ServiceCardBatch> Repository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceCardBatchDeleteArguments>.ExecuteAsync(ServiceCardBatchDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
