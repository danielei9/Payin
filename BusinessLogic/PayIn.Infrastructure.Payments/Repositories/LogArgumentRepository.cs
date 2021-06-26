using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Payments
{
	public class LogArgumentRepository : PublicRepository<LogArgument>
	{
		#region Contructors
		public LogArgumentRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
