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
	public class EntranceTypeDeleteHandler :
		IServiceBaseHandler<EntranceTypeDeleteArguments>
	{
		[Dependency] public IEntityRepository<EntranceType> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroupEntranceType> ServiceGroupEntranceTypeRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = EntranceTypeState.Deleted;

			var linkedServiceGroups = (await ServiceGroupEntranceTypeRepository.GetAsync())
				.Where(x =>
					x.EntranceTypeId == arguments.Id
				);
			await ServiceGroupEntranceTypeRepository.DeleteAsync(linkedServiceGroups);

			return null;
		}
		#endregion ExecuteAsync
	}
}
