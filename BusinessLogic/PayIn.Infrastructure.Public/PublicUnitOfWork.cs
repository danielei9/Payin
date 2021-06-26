using PayIn.Infrastructure.Public.Db;
using Xp.Infrastructure;

namespace PayIn.Infrastructure.Public
{
	public class PublicUnitOfWork : UnitOfWork<IPublicContext>
	{
		#region Constructors
		public PublicUnitOfWork(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
