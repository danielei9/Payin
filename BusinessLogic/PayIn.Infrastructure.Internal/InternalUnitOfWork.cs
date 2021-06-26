using PayIn.Infrastructure.Internal.Db;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Internal
{
	public class InternalUnitOfWork : UnitOfWork<IInternalContext>
	{
		#region Constructors
		public InternalUnitOfWork(IInternalContext context)
			:base(context)
		{
		}
		#endregion Constructors
	}
}
