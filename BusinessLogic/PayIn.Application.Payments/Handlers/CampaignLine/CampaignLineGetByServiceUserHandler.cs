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
	public class CampaignLineGetByServiceUserHandler :
		IQueryBaseHandler<CampaignLineGetByServiceUserArguments, CampaignLineGetByServiceUserResult>
	{
		[Dependency] public IEntityRepository<CampaignLineServiceUser> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineGetByServiceUserResult>> ExecuteAsync(CampaignLineGetByServiceUserArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.CampaignLineId == arguments.Id)
				.Select(x => new CampaignLineGetByServiceUserResult
				{
					Id = x.ServiceUserId,
					Name = x.ServiceUser.Name + " " + x.ServiceUser.LastName
				});

			return new ResultBase<CampaignLineGetByServiceUserResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
