using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class NoticeVisibilityHandler :
		IServiceBaseHandler<NoticeVisibilityArguments>
	{
		private readonly IEntityRepository<Notice> Repository;

		#region Constructors
		public NoticeVisibilityHandler(IEntityRepository<Notice> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NoticeVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Visibility = arguments.Visibility;

			return item;
		}
		#endregion ExecuteAsync
	}
}