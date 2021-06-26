using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handler
{
	public class NoticeGetVisibilityHandler :
		IQueryBaseHandler<NoticeGetVisibilityArguments, NoticeGetVisibilityResult>
	{
		private readonly IEntityRepository<Notice> Repository;

		#region Constructors
		public NoticeGetVisibilityHandler(
			IEntityRepository<Notice> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<NoticeGetVisibilityResult>> ExecuteAsync(NoticeGetVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new NoticeGetVisibilityResult
				{
					Id = x.Id,
					Visibility = x.Visibility,
				});

			return new ResultBase<NoticeGetVisibilityResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
