using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeDeleteGroupHandler :
		IServiceBaseHandler<EntranceTypeDeleteGroupArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroupEntranceType> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeDeleteGroupArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id && x.GroupId == arguments.GroupId);

			foreach (var item in items)
				await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
