using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeGroupsGetAllHandler :
		IQueryBaseHandler<EntranceTypeGroupsGetAllArguments, EntranceTypeGroupsGetAllResult>
	{
		[Dependency] public IEntityRepository<EntranceType> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeGroupsGetAllResult>> ExecuteAsync(EntranceTypeGroupsGetAllArguments arguments)
		{
			var entranceType = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new {
					x.Name,
					EventName = x.Event.Name
				})
				.FirstOrDefault();

			var items = (await ServiceGroupRepository.GetAsync())
				.Where(x => x.EntranceTypes.Any(y => y.EntranceTypeId == arguments.Id))
				.Select(x => new EntranceTypeGroupsGetAllResult
				{
					Id = x.Id,
					Name = x.Name
				});
			
			return new EntranceTypeGroupsGetAllResultBase {
				EntranceTypeName = entranceType?.Name ?? "",
				EventName = entranceType.EventName ?? "",
				Data = items
			};
		}
		#endregion ExecuteAsync
	}
}
