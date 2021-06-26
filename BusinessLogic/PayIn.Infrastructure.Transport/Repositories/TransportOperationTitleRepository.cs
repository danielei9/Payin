using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PayIn.Infrastructure.Transport.Repositories
{
	class TransportOperationTitleRepository : PublicRepository<TransportOperationTitle>
	{
		#region Contructors
		public TransportOperationTitleRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportOperationTitle> CheckHorizontalVisibility(IQueryable<TransportOperationTitle> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
