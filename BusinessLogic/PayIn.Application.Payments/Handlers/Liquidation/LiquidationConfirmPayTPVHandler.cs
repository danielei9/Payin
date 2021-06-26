using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class LiquidationConfirmPayTPVHandler : IServiceBaseHandler<LiquidationConfirmPayTPVArguments>
	{
		private readonly IEntityRepository<Liquidation> Repository;

		#region Constructors
		public LiquidationConfirmPayTPVHandler(
			IEntityRepository<Liquidation> repository
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<LiquidationConfirmPayTPVArguments>.ExecuteAsync(LiquidationConfirmPayTPVArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.PaidTPV = true;

			return item;
		}
		#endregion ExecuteAsync
	}
}

