using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal
{
	public class LogArgumentRepository : InternalRepository<LogArgument>
	{
		#region Contructors
		public LogArgumentRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
