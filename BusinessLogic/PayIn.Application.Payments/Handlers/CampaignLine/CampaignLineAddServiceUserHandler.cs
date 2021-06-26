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
	public class CampaignLineAddServiceUserHandler : 
		IServiceBaseHandler<CampaignLineAddServiceUserArguments>
	{
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<CampaignLineServiceUser> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineAddServiceUserArguments arguments)
		{
			var item = new CampaignLineServiceUser
			{
				CampaignLineId = arguments.Id,
				ServiceUserId = arguments.ServiceUserId
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
