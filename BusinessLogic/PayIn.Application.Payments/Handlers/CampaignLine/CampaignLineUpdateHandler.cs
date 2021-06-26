using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineUpdateHandler :
		IServiceBaseHandler<CampaignLineUpdateArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;

		#region Constructors
		public CampaignLineUpdateHandler(IEntityRepository<CampaignLine> repository)
		{
			if (repository == null)	throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<CampaignLineUpdateArguments>.ExecuteAsync(CampaignLineUpdateArguments arguments)
		{
			var now = DateTime.Now;
			var item = await Repository.GetAsync(arguments.Id, "Campaign");

			if (arguments.Min == 0 && arguments.Max == 0)
				arguments.Max = 999999;

				item.Max = arguments.Max;
				item.Min = arguments.Min;
				item.Quantity = arguments.Quantity;
				item.Type = arguments.Type;
				item.PurseId = arguments.PurseId;
				item.AllProduct = arguments.AllProduct;
				item.AllEntranceType = arguments.AllEntranceType;
				item.UntilTime = arguments.All ? null : arguments.UntilTime;
				item.SinceTime = arguments.All ? null : arguments.SinceTime;

				return item;
		}
			
		#endregion ExecuteAsync
	}
}
