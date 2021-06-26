using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportCardRepository : PublicRepository<TransportCard>
	{

		#region Contructors
		public TransportCardRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportCard> CheckHorizontalVisibility(IQueryable<TransportCard> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
