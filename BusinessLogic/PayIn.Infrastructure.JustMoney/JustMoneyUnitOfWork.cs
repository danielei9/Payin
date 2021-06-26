using PayIn.Infrastructure.JustMoney.Db;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.JustMoney
{
	public class JustMoneyUnitOfWork : UnitOfWork<IJustMoneyContext>
	{
		#region Constructors
		public JustMoneyUnitOfWork(IJustMoneyContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
