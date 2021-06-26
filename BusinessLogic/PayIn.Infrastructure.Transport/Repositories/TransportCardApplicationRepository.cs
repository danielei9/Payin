using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportCardApplicationRepository : PublicRepository<TransportCardApplication>
	{

		#region Contructors
		public TransportCardApplicationRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportCardApplication> CheckHorizontalVisibility(IQueryable<TransportCardApplication> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
