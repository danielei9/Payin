using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class LiquidationRemoveAccountLinesHandler :
		IServiceBaseHandler<LiquidationRemoveAccountLinesArguments>
    {
        [Dependency] public IEntityRepository<AccountLine> AccountLinesRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(LiquidationRemoveAccountLinesArguments arguments)
        {
            var now = DateTime.UtcNow;

            var lines = (await AccountLinesRepository.GetAsync())
                .Where(x =>
                    x.Id == arguments.AccountLineId &&
                    x.LiquidationId == arguments.Id &&
                    x.Liquidation.State == LiquidationState.Opened
                );
            foreach(var line in lines)
                line.LiquidationId = null;

            return new { arguments.Id } ;
        }
		#endregion ExecuteAsync
	}
}
