using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportCardSupportRepository : PublicRepository<TransportCardSupport>
	{

		#region Contructors
		public TransportCardSupportRepository(
			IPublicContext context
		)
			: base(context)
		{

		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportCardSupport> CheckHorizontalVisibility(IQueryable<TransportCardSupport> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
