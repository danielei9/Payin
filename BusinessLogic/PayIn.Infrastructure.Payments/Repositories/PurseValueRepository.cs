using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class PurseValueRepository : PublicRepository<PurseValue>
	{
        #region Contructors
        public PurseValueRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
