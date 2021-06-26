using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class LiquidationCreateAccountLinesHandler :
		IServiceBaseHandler<LiquidationCreateAccountLinesArguments>
    {
        [Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<AccountLine> AccountLinesRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(LiquidationCreateAccountLinesArguments arguments)
		{
            var now = DateTime.UtcNow;

            var ids = arguments.Lines
                .Select(x => x.Id);
            var lines = (await AccountLinesRepository.GetAsync("Ticket"))
                .Where(x => ids.Contains(x.Id))
                .ToList();

            var concessionId = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Concession.Supplier.Login == SessionData.Login)
                )
                .Select(x => (int?)x.Id)
                .FirstOrDefault();
            if (concessionId == null)
                throw new ApplicationException("Empresa liquidadora no encontrada");

            var liquidation = new Liquidation(lines, concessionId.Value, now);
            await Repository.AddAsync(liquidation);
            
            return liquidation;
		}
		#endregion ExecuteAsync
	}
}
