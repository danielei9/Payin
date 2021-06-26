using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Event", "MobileGetMap")]
	public class MobileEventGetMapHandler :
		IQueryBaseHandler<MobileEventGetMapArguments, MobileEventGetMapResult>
	{		
		[Dependency] public IEntityRepository<Event> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEventGetMapResult>> ExecuteAsync(MobileEventGetMapArguments arguments)
		{
			var now = DateTime.UtcNow;

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State == EventState.Active) &&
					(x.IsVisible)
				)
				.Select(x => new MobileEventGetMapResult
				{
					Id = x.Id,
					MapUrl = x.MapUrl
                })
#if DEBUG
				.ToList()
#endif // DEBUG
				;
            
			return new ResultBase<MobileEventGetMapResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
