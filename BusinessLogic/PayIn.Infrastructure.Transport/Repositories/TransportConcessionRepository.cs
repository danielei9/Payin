using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportConcessionRepository : PublicRepository<TransportConcession>
	{

		#region Contructors
		public TransportConcessionRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportConcession> CheckHorizontalVisibility(IQueryable<TransportConcession> that)
		{
			that = that
				.Where(x =>
					x.Concession.Concession.State == ConcessionState.Active);
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
