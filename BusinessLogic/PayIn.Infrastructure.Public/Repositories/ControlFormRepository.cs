using PayIn.BusinessLogic.Common;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayIn.Domain.Public;
using PayIn.Domain.Security;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlFormRepository : PublicRepository<ControlForm>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public ControlFormRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors
	}
}
