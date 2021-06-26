using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Liquidation", "Open")]
	public class LiquidationOpenHandler :
		IServiceBaseHandler<LiquidationOpenArguments>
    {
		[Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationOpenArguments arguments)
        {
            var liquidation = (await Repository.GetAsync("Concession.Concession"))
                .Where(x =>
                    (x.State == LiquidationState.Closed) &&
                    (x.Id == arguments.Id)
                )
                .FirstOrDefault();
            if (liquidation == null)
                throw new ArgumentNullException(nameof(LiquidationCloseArguments.Id));

            liquidation.State = LiquidationState.Opened;

            return liquidation;
		}
		#endregion ExecuteAsync
	}
}
