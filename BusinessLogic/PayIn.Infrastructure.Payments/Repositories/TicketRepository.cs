using Microsoft.Practices.Unity;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;
using Xp.Domain;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class TicketRepository : PublicRepository<Ticket>
	{
		public readonly ISessionData SessionData;
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }

		#region Constructors
		public TicketRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region CheckHorizontalVisibility
		public override IQueryable<Ticket> CheckHorizontalVisibility(IQueryable<Ticket> that)
		{
			var task = SystemCardRepository.GetAsync();
			task.Wait();
			var myOwnedSystemCards = task.Result
				.Where(x =>
					x.ConcessionOwner.Supplier.Login == SessionData.Login
				);

			var task2 = PaymentConcessionRepository.GetAsync();
			task2.Wait();
			var myConcessions = task2.Result
				.Where(x =>
					x.Concession.Type == ServiceType.Charge &&
					x.Concession.State == ConcessionState.Active &&
					(
						x.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentWorkers.Any(y => y.Login == SessionData.Login) ||
						myOwnedSystemCards
							.Any(y =>
								y.SystemCardMembers.Any(z => z.Login == x.Concession.Supplier.Login)
							)
					)
				);


			var result = that;
			
			var now = DateTime.Now;
			if (SessionData.Login.IsNullOrEmpty()) // Para llamadas desde el Timer
			{
				result = result
					.Where(x =>
						x.Payments
						.Where(y =>
							y.State == PaymentState.Active &&
							y.LiquidationId == null
						)
						.Any()
					);
			}
			else
			{
				result = that
				.Where(x =>
					x.Concession.Concession.Type == ServiceType.Charge &&
					x.Concession.Concession.State == ConcessionState.Active &&
					(
						// Publico en general en sin pagar
						(
							x.Payments.Where(y => y.State == PaymentState.Active).Count() == 0 && // sin pagar
							(
								(x.PaymentUserId == null && now < x.Until) || // pago normal no caducado
								x.PaymentUser.Login == SessionData.Login // recibo para mi
							) &&
							now >= x.Since
						) ||
						// Amo del bar
						x.Concession.Concession.Supplier.Login == SessionData.Login ||
						// Camarero que ha generado el ticket
						(
							x.PaymentWorker.Login == SessionData.Login &&
							x.PaymentWorker.State == WorkerState.Active
						) ||
						// Concesiones de un sistema de tarjetas del que soy propietario
						(
							myConcessions.Any(y => y.Id == x.ConcessionId)
						) ||
						// Otros camareros
						(
							(
                                x.Concession.PaymentWorkers
                                .Any(y =>
								    y.Login == SessionData.Login &&
								    y.State == WorkerState.Active
							    )
                            ) &&
							(now < x.Until) &&
                            (
                                (x.Type == TicketType.NotPayable) || // Para metro para poder ver los tickets desde el movil generados desde la web
                                (
                                    !x.Payments
                                    .Where(y => 
                                        !(new[] { PaymentState.Pending })
                                            .Contains(y.State)
                                    )
                                    .Any()
                                )
                            )
						) ||
						// Pagador
						x.Payments.Any(y => y.UserLogin == SessionData.Login) ||
						// Superadministrador
						(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
						// Soy el dueño de la tarjeta que hace la operación
						x.ServiceOperations.Any(y => y.Card.OwnerUser.Login == SessionData.Login) ||
						// Tengo vinculada la tarjeta que hace la operación
						x.ServiceOperations.Any(y => y.Card.LinkedUsers
							.Any(z => z.Login == SessionData.Login)
						)||
						// Algun pago del ticket es mio
						x.Payments.Any(y => y.UserLogin == SessionData.Login)
					)
				);
			}

			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
