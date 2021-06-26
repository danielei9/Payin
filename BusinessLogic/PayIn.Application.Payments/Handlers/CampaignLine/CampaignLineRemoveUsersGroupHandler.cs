using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineRemoveServiceGroupHandler :
		IServiceBaseHandler<CampaignLineRemoveServiceGroupArguments>
	{
		[Dependency] public IEntityRepository<CampaignLineServiceGroup> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineRemoveServiceGroupArguments arguments)
		{
            var item = (await Repository.GetAsync())
                .Where(x =>
                    x.CampaignLineId == arguments.Id &&
                    x.ServiceGroupId == arguments.ServiceGroupId
                );

            await Repository.DeleteAsync(item);

            return null;
		}
		#endregion ExecuteAsync
	}
}
