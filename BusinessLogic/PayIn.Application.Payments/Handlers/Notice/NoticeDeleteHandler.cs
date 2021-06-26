using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class NoticeDeleteHandler :
		IServiceBaseHandler<NoticeDeleteArguments>
	{
		private readonly IEntityRepository<Notice> Repository;

		#region Constructors
		public NoticeDeleteHandler(
			IEntityRepository<Notice> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NoticeDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x =>
				   x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = NoticeState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
