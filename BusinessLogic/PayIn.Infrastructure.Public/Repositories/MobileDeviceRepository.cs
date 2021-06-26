using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class MobileDeviceRepository : PublicRepository<Device>
	{
		#region Constructors
		public MobileDeviceRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
