using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using PayIn.Infrastructure.Bus.Db;
using System;

namespace PayIn.Infrastructure.Bus.Repositories
{
	public class RouteRepository : BusRepository<Route>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public RouteRepository(
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
