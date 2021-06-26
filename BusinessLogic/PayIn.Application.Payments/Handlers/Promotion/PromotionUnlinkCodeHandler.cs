using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Domain.Promotions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionUnlinkCodeHandler : IServiceBaseHandler<PromotionUnlinkCodeArguments>
	{
		private readonly IEntityRepository<PromoExecution> Repository;

		#region Constructors
		public PromotionUnlinkCodeHandler(
			IEntityRepository<PromoExecution> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionUnlinkCodeArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.PromoUserId = null;

			return null;
		}
		#endregion ExecuteAsync
	}
}

