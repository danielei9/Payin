using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Liquidation", "ConfirmUnpay")]
	public class LiquidationConfirmUnpayHandler  :
		IServiceBaseHandler<LiquidationConfirmUnpayArguments>
	{
		private readonly IEntityRepository<Liquidation> Repository;

		#region Constructors
		public LiquidationConfirmUnpayHandler(
			IEntityRepository<Liquidation> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationConfirmUnpayArguments arguments)
		{
			var liquidation = (await Repository.GetAsync(arguments.Id));

			liquidation.PaymentDate = null;
			liquidation.State = LiquidationState.Closed;

			return liquidation;
		}
		#endregion ExecuteAsync
	}
}
