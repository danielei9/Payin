using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class ExhibitorRepository : PublicRepository<Exhibitor>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ExhibitorRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<Exhibitor> CheckHorizontalVisibility(IQueryable<Exhibitor> that)
        {
            return that;
        }
        #endregion CheckHorizontalVisibility
    }
}
