using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	class TransportValidationRepository : PublicRepository<TransportValidation>
	{
		#region Contructors
		public TransportValidationRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportValidation> CheckHorizontalVisibility(IQueryable<TransportValidation> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
