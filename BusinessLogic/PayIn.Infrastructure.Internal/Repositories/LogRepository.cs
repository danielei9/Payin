using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal
{
	public class LogRepository : InternalRepository<Log>
	{
		#region Contructors
		public LogRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
