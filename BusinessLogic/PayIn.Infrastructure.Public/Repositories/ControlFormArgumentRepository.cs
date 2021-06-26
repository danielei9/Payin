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
	public class ControlFormArgumentRepository : PublicRepository<ControlFormArgument>
	{
		#region Constructors
		public ControlFormArgumentRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}