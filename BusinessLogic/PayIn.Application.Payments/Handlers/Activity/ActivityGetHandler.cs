using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class ActivityGetHandler :
		IQueryBaseHandler<ActivityGetArguments, ActivityGetResult>
	{
		private readonly IEntityRepository<Activity> Repository;

		#region Constructors
		public ActivityGetHandler(
			IEntityRepository<Activity> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ActivityGetResult>> ExecuteAsync(ActivityGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Start = (x.Start == XpDateTime.MinValue) ? null : (DateTime?)x.Start,
					End = (x.End == XpDateTime.MaxValue) ? null : (DateTime?)x.End,
				})
				.ToList()
				.Select(x => new ActivityGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Start = x.Start.ToUTC(),
					End = x.End.ToUTC()
				});
			return new ResultBase<ActivityGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
