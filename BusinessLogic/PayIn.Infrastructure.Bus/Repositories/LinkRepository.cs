using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using PayIn.Infrastructure.Bus.Db;
using System;

namespace PayIn.Infrastructure.Bus.Repositories
{
	public class LinkRepository : BusRepository<Link>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public LinkRepository(
			ISessionData sessionData,
			IBusContext context
		)
			: base(context)
		{
			SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
		}
		#endregion Constructors
	}
}
