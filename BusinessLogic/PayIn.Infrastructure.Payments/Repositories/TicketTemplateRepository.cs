using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class TicketTemplateRepository : PublicRepository<TicketTemplate>
	{
		#region Contructors
		public TicketTemplateRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
