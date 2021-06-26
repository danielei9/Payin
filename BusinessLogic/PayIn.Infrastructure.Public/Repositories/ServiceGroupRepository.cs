using PayIn.Infrastructure.Public.Db;
using PayIn.Domain.Public;
using System.Linq;
using PayIn.BusinessLogic.Common;
using System;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceGroupRepository : PublicRepository<ServiceGroup>
	{
		#region Constructors
		public ServiceGroupRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
