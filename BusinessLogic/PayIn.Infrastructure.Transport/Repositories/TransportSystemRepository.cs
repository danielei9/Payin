using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportSystemRepository : PublicRepository<TransportSystem>
	{

		#region Contructors
		public TransportSystemRepository(
			IPublicContext context
		)
			: base(context)
		{

		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportSystem> CheckHorizontalVisibility(IQueryable<TransportSystem> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
