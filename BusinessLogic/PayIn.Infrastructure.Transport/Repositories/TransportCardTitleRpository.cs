using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportCardTitleRepository : PublicRepository<TransportCardTitle>
	{

		#region Contructors
		public TransportCardTitleRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportCardTitle> CheckHorizontalVisibility(IQueryable<TransportCardTitle> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
