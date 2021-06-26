using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	class TransportOperationRepository: PublicRepository<TransportOperation>
	{
		#region Contructors
		public TransportOperationRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportOperation> CheckHorizontalVisibility(IQueryable<TransportOperation> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
