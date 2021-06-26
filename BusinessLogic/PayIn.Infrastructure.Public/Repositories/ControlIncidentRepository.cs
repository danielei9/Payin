using PayIn.Infrastructure.Public.Db;
using PayIn.Domain.Public;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlIncidentRepository : PublicRepository<ControlIncident>
	{
		#region Constructors
		public ControlIncidentRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
