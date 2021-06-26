using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileContactGetAllHandler :
		IQueryBaseHandler<MobileContactGetAllArguments, MobileContactGetAllResult>
	{
		[Dependency] public IEntityRepository<Contact> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileContactGetAllResult>> ExecuteAsync(MobileContactGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.VisitorLogin == SessionData.Login &&
					x.State != ContactState.Deleted &&
					x.Event.State == EventState.Active &&
					x.Event.IsVisible == true &&
					x.Exhibitor.State == ExhibitorState.Active
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.VisitorLogin.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new MobileContactGetAllResult
				{
					Id = x.Id,
					ExhibitorId = x.ExhibitorId,
					ExhibitorName = x.Exhibitor.Name,
					VisitorName = x.VisitorEntrance.UserName + " " + x.VisitorEntrance.LastName,
					VisitorEntranceId = x.VisitorEntranceId,
					EventId = x.EventId,
					EventName = x.Event.Name,
					EventPhotoUrl = x.Event.PhotoUrl,
					State = x.State
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;
			return new ResultBase<MobileContactGetAllResult> { Data = result };
 		}
		#endregion ExecuteAsync
	}
}
