using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportCardSupportTitleCompatibilityRepository : PublicRepository<TransportCardSupportTitleCompatibility>
	{

		#region Contructors
		public TransportCardSupportTitleCompatibilityRepository(
			IPublicContext context
		)
			: base(context)
		{

		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportCardSupportTitleCompatibility> CheckHorizontalVisibility(IQueryable<TransportCardSupportTitleCompatibility> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
