using Microsoft.Practices.Unity;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
    public class AccountLineRepository : PublicRepository<AccountLine>
	{
		[Dependency] public ISessionData SessionData { get; set; }

		#region Contructors
		public AccountLineRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<AccountLine> CheckHorizontalVisibility(IQueryable<AccountLine> that)
		{
            return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
