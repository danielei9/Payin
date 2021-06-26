using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineCreateHandler :
		IServiceBaseHandler<CampaignLineCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<CampaignLine> Repository;	
		
		#region Constructors
		public CampaignLineCreateHandler(
			ISessionData sessionData,
			IEntityRepository<CampaignLine> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineCreateArguments arguments)
		{			
			if (arguments.Min == 0 && arguments.Max == 0)
				arguments.Max = 999999;

			if ((arguments.Min > arguments.Max) && arguments.Max != 0)
				throw new Exception(CampaignLineResources.MinMaxException);

			if (arguments.Quantity > 100 || arguments.Quantity < 0)
				throw new Exception(CampaignLineResources.IntervalException);
		
			var campaignLines = (await Repository.GetAsync())
				.Where(x => x.CampaignId == arguments.CampaignId );

			if (campaignLines.Where(x => 
									arguments.Max >= x.Min && 
									arguments.Min <= x.Max && 
									x.State == CampaignLineState.Active && 
									(x.SinceTime <= arguments.SinceTime || 
										x.SinceTime == null)  && 
									(x.UntilTime > arguments.UntilTime || 
										x.UntilTime == null))
									.Count() > 0)
				throw new Exception(CampaignLineResources.IntervalOverlapException);

				var campaignLine = new CampaignLine
				{
					Type = arguments.Type,
					Min = 0,
					Max = int.MaxValue,
					Quantity = arguments.Quantity,
					SinceTime = arguments.SinceTime,
					UntilTime = arguments.UntilTime,
					CampaignId = arguments.CampaignId,
					PurseId = arguments.PurseId,
					State = CampaignLineState.Active,
					AllProduct = arguments.AllProduct,
					AllEntranceType = arguments.AllEntranceType
				};
					await Repository.AddAsync(campaignLine);
					return campaignLine;
		}
		#endregion ExecuteAsync
	}
}

