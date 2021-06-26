using PayIn.BusinessLogic.Common;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayIn.Domain.Public;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class DeviceRepository : PublicRepository<Device>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public DeviceRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region CheckHorizontalVisibility
		public override IQueryable<Device> CheckHorizontalVisibility(IQueryable<Device> that)
		{
			var result = that
				//.Where(x => x.Login == SessionData.Login)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
