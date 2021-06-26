using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class LiquidationAddAccountLinesHandler :
		IServiceBaseHandler<LiquidationAddAccountLinesArguments>
    {
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public IEntityRepository<AccountLine> AccountLinesRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(LiquidationAddAccountLinesArguments arguments)
        {
            var now = DateTime.UtcNow;

            var ids = arguments.Lines
                .Select(x => x.Id);
            var lines = (await AccountLinesRepository.GetAsync("Ticket"))
                .Where(x => ids.Contains(x.Id))
                .ToList();

            var liquidation = await Repository.GetAsync(arguments.Id, "AccountLines.Ticket");
            liquidation.AddAccountLines(lines, now);

            return liquidation;
        }
		#endregion ExecuteAsync
	}
}
