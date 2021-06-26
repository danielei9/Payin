using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Liquidation", "CreateAndPay")]
	public class LiquidationCreateAndPayHandler :
		IServiceBaseHandler<LiquidationCreateAndPayArguments>
	{
        [Dependency] public IEntityRepository<Liquidation> Repository { get; set; }
        [Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LiquidationCreateAndPayArguments arguments)
		{
			var now = DateTime.Now;

            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

            var concession = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Id == arguments.ConcessionId) &&
                    (
                        // Liquiaciones mias
                        (x.Concession.Supplier.Login == SessionData.Login) ||
                        // Liquidaciones de las empresas de mi sistema de tarjetas
                        (
                            systemCardMembers
                                .Any(y =>
                                    y.Login == x.Concession.Supplier.Login &&
                                    y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
                                )
                        )
                    )
                )
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Concession.Name,
                    Payments = x.Tickets
                        .SelectMany(y =>
                            y.Payments
                                .Where(z =>
                                    (z.State == PaymentState.Active) &&
                                    (z.LiquidationId == null)
                                )
                        )
                })
                .FirstOrDefault();
            if (concession == null)
                throw new ArgumentNullException("concessionId");
            
			var liquidation = new Liquidation
			{
				State = LiquidationState.Payed,
				Until = now.ToUTC(),
				Since = concession.Payments.Min(x => x.Date).ToUTC(),
				TotalQuantity = concession.Payments.Sum(x => (decimal?)x.Amount) ?? 0,
				PayinQuantity = concession.Payments.Sum(x => (decimal?)x.Payin) ?? 0,
				PaidQuantity = (concession.Payments.Sum(x => (decimal?)x.Amount) ?? 0) - (concession.Payments.Sum(x => (decimal?)x.Payin) ?? 0),
				PaymentDate = now.ToUTC(),
				RequestDate = now.ToUTC(),
				ConcessionId = concession.Id,
				Payments = concession.Payments.ToList(),
				Recharges = new Collection<Recharge>()
			};
            await Repository.AddAsync(liquidation);

			return liquidation;
		}
		#endregion ExecuteAsync
	}
}
