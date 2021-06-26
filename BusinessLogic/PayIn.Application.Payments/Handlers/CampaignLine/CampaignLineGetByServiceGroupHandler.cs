using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineGetByServiceGroupHandler :
		IQueryBaseHandler<CampaignLineGetByServiceGroupArguments, CampaignLineGetByServiceGroupResult>
	{
		[Dependency] public IEntityRepository<CampaignLineServiceGroup> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineGetByServiceGroupResult>> ExecuteAsync(CampaignLineGetByServiceGroupArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.CampaignLineId == arguments.Id)
				.Select(x => new CampaignLineGetByServiceGroupResult
				{
					Id = x.ServiceGroupId,
					Name = x.ServiceGroup.Name
				});

			return new ResultBase<CampaignLineGetByServiceGroupResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
