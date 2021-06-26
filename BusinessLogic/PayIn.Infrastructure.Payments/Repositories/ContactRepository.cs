using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class ContactRepository : PublicRepository<Contact>
	{
		#region Contructors
		public ContactRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
