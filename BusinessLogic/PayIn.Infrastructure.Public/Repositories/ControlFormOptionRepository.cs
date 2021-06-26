using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ControlFormOptionRepository : PublicRepository<ControlFormOption>
	{
		public ISessionData SessionData { get; set; }

		#region Constructors
		public ControlFormOptionRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
