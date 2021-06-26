using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class CampaignLineAddServiceGroupHandler : 
		IServiceBaseHandler<CampaignLineAddServiceGroupArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<CampaignLineServiceGroup> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineAddServiceGroupArguments arguments)
		{
			var item = new CampaignLineServiceGroup
			{
				CampaignLineId = arguments.Id,
                ServiceGroupId = arguments.ServiceGroupId
            };
			await Repository.AddAsync(item);

			return item;			
		}
		#endregion ExecuteAsync
	}
}
