using Microsoft.Practices.Unity;
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
	[XpLog("Liquidation", "Pay")]
	public class LiquidationPayHandler  :
		IServiceBaseHandler<LiquidationPayArguments>
	{
		[Dependency] public IEntityRepository<Liquidation> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationPayArguments arguments)
		{
			var now = DateTime.UtcNow;

			var liquidation = (await Repository.GetAsync(arguments.Id));

			liquidation.PaymentDate = now;
			liquidation.State = LiquidationState.Payed;

			return liquidation;
		}
		#endregion ExecuteAsync
	}
}
