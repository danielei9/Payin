using Microsoft.Practices.Unity;
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
    class MobileActivityGetByEventHandler :
		IQueryBaseHandler<MobileActivityGetByEventArguments, MobileActivityGetByEventResult>
	{
		[Dependency] public IEntityRepository<Activity> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileActivityGetByEventResult>> ExecuteAsync(MobileActivityGetByEventArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.EventId == arguments.Id)
				)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
                    x.Description,
					Start = (x.Start == XpDateTime.MinValue) ? null : (DateTime?)x.Start,
					End = (x.End == XpDateTime.MinValue) ? null : (DateTime?)x.End
				})
				.ToList()
				.Select(x => new MobileActivityGetByEventResult
                {
					Id = x.Id,
					Name = x.Name,
                    Description = x.Description,
                    Start = x.Start?.ToUTC(),
					End = x.End?.ToUTC()
				});
			return new ResultBase<MobileActivityGetByEventResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
