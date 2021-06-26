using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class NoticeImageRepository : PublicRepository<NoticeImage>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public NoticeImageRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<NoticeImage> CheckHorizontalVisibility(IQueryable<NoticeImage> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
