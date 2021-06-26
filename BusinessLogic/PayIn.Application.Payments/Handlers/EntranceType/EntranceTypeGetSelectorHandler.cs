using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class EntranceTypeGetSelectorHandler :
		IQueryBaseHandler<EntranceTypeGetSelectorArguments, EntranceTypeGetSelectorResult>
	{
		[Dependency] public IEntityRepository<EntranceType> Repository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeGetSelectorResult>> ExecuteAsync(EntranceTypeGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.State == Common.EntranceTypeState.Active
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);
			
			var result = items
				.Select(x => new EntranceTypeGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name +" ("+ x.Event.Name +")"
				});

			return new ResultBase<EntranceTypeGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}