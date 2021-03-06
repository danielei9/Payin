using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardBatchLockHandler :
		IServiceBaseHandler<ServiceCardBatchLockArguments>
	{
		[Dependency] public IEntityRepository<ServiceCardBatch> Repository { get; set; }


		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardBatchLockArguments arguments)
		{

			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ServiceCardBatchState.Blocked;

			return item;
		}
		#endregion ExecuteAsync
	}
}
