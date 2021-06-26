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
	class ActivityDeleteHandler :
		IServiceBaseHandler<ActivityDeleteArguments>
	{
		private readonly IEntityRepository<Activity> Repository;

		#region Constructors
		public ActivityDeleteHandler(IEntityRepository<Activity> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ActivityDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
