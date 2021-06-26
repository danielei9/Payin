using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiCampaignUnsuspendHandler :
		IServiceBaseHandler<ApiCampaignUnsuspendArguments>
	{
		private readonly IEntityRepository<Campaign> Repository;

		#region Constructors
		public ApiCampaignUnsuspendHandler(IEntityRepository<Campaign> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiCampaignUnsuspendArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = CampaignState.Active;

			return item;
		}
		#endregion ExecuteAsync
	}
}
