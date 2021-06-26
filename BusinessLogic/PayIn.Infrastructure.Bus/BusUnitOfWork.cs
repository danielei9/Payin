using PayIn.Infrastructure.Bus.Db;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Bus
{
	public class BusUnitOfWork : UnitOfWork<IBusContext>
	{
		#region Constructors
		public BusUnitOfWork(IBusContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
