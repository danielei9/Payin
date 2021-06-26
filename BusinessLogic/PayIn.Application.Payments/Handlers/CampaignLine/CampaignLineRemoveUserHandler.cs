using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineRemoveServiceUserHandler :
		IServiceBaseHandler<CampaignLineRemoveServiceUserArguments>
	{
		[Dependency] public IEntityRepository<CampaignLineServiceUser> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineRemoveServiceUserArguments arguments)
		{
			var item = (await Repository.GetAsync())
                .Where(x =>
                    x.CampaignLineId == arguments.Id &&
                    x.ServiceUserId == arguments.ServiceUserId
                );

            await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
