using System;
using System.Linq;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using Xp.Domain;

namespace PayIn.Infrastructure.Public.Repositories
{
    public class PurseRepository : PublicRepository<Purse>
	{
        public readonly ISessionData SessionData;
        public readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;

        #region Contructors
        public PurseRepository(
			ISessionData sessionData,
			IPublicContext context,
            IEntityRepository<PaymentConcession> paymentConcessionRepository
        )
			: base(context)
        {
            SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
            PaymentConcessionRepository = paymentConcessionRepository ?? throw new ArgumentNullException("paymentConcessionRepository");
        }
		#endregion Contructors

		//#region CheckHorizontalVisibility
		//public override IQueryable<Purse> CheckHorizontalVisibility(IQueryable<Purse> that)
		//{
		//	var now = DateTime.Now;

  //          that = that
		//	    .Where(x =>
		//		    // Usuario
		//		    (x.PaymentMedias.Any(y => y.Login == SessionData.Login) && x.Expiration >=  now) ||
		//		    // Empresa dueña del monedero, o añadida al monedero
		//		    (x.Concession.Concession.Supplier.Login == SessionData.Login) ||
		//		    (x.PaymentConcessionPurses.Any(y => y.PaymentConcession.Concession.Supplier.Login == SessionData.Login)) ||
		//			// Asociado a un SystemCard
		//			(x.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login) ||
		//			(x.SystemCard.SystemCardMembers
		//				.Any(y =>
  //                          (y.Login == SessionData.Login) ||
  //                          (paymentConcession) // faltaria aqui los trabajadores...
  //                      )
		//			) ||
		//			(x.SystemCard.Cards
		//				.Any(y =>
		//					(y.OwnerLogin == SessionData.Login) || // Vinculadas
		//					(y.OwnerUser.Login == SessionData.Login) // Propiedad
		//				)
		//			)
		//		);
		//	return that;
		//}
		//#endregion CheckHorizontalVisibility
	}
}
