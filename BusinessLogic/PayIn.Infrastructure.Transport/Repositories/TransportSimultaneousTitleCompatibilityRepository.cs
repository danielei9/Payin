using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportSimultaneousTitleCompatibilityRepository : PublicRepository<TransportSimultaneousTitleCompatibility>
	{
		#region Contructors
		public TransportSimultaneousTitleCompatibilityRepository(
			IPublicContext context
		)
			: base(context)
		{

		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportSimultaneousTitleCompatibility> CheckHorizontalVisibility(IQueryable<TransportSimultaneousTitleCompatibility> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
