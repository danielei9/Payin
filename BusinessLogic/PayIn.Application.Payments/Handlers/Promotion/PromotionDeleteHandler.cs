using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Common;
using PayIn.Domain.Promotions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class PromotionDeleteHandler : IServiceBaseHandler<PromotionDeleteArguments>
	{
		private readonly IEntityRepository<Promotion> Repository;

		#region Constructors
		public PromotionDeleteHandler(
			IEntityRepository<Promotion> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PromotionDeleteArguments>.ExecuteAsync(PromotionDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = PromotionState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

