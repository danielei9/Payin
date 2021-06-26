using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class ApiActivityGetAllHandler :
		IQueryBaseHandler<ApiActivityGetAllArguments, ApiActivityGetAllResult>
	{
		private readonly IEntityRepository<Activity> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ApiActivityGetAllHandler(
			IEntityRepository<Activity> repository,
			ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiActivityGetAllResult>> ExecuteAsync(ApiActivityGetAllArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.EventId == arguments.Id
				)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Start = (x.Start == XpDateTime.MinValue) ? null : (DateTime?)x.Start,
					End = (x.End == XpDateTime.MinValue) ? null : (DateTime?)x.End,
				})
				.ToList()
				.Select(x => new ApiActivityGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Start = x.Start,
					End = x.End
				});
			return new ResultBase<ApiActivityGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
