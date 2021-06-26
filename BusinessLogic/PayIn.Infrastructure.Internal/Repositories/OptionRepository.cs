using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal.Repositories
{
	public class OptionRepository : InternalRepository<Option>
	{
		#region Contructors
		public OptionRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
