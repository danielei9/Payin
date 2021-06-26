using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class NoticeIsVisibleHandler :
		UpdateHandler<NoticeIsVisibleArguments, Notice>
	{
		#region Constructors
		public NoticeIsVisibleHandler(IEntityRepository<Notice> repository)
			: base(repository)
		{
		}
		#endregion Constructors
	}
}