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
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Liquidation", "Close")]
    public class LiquidationCloseHandler :
		IServiceBaseHandler<LiquidationCloseArguments>
    {
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public EmailService EmailService { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(LiquidationCloseArguments arguments)
		{
            var liquidation = (await Repository.GetAsync("Concession.Concession"))
                .Where(x =>
                    (x.State == LiquidationState.Opened) &&
                    (x.Id == arguments.Id)
                )
                .FirstOrDefault();
            if (liquidation == null)
                throw new ArgumentNullException(nameof(LiquidationCloseArguments.Id));

            liquidation.State = LiquidationState.Closed;
            
            await EmailService.SendAsync(
                "info@pay-in.es",
                "Liquidación cerrada de la empresa ".FormatString(liquidation.Concession.Concession.Name),
                ""
            );
            
            return liquidation;
		}
		#endregion ExecuteAsync
	}
}
