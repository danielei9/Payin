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
	public class MobileExhibitorGetAllVisitorsHandler :
		IQueryBaseHandler<MobileExhibitorGetAllVisitorsArguments, MobileExhibitorGetAllVisitorsResult>
	{
		private readonly IEntityRepository<Contact> ContactRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public MobileExhibitorGetAllVisitorsHandler(
			IEntityRepository<Contact> contactRepository,
			ISessionData sessionData
		)
		{
			if (contactRepository == null) throw new ArgumentNullException("contactRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			
			ContactRepository = contactRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<MobileExhibitorGetAllVisitorsResult>> ExecuteAsync(MobileExhibitorGetAllVisitorsArguments arguments)
		{
			var result = (await ContactRepository.GetAsync())
				.Where(x =>
					x.Exhibitor.State == ExhibitorState.Active &&
					x.Exhibitor.PaymentConcession.Concession.State == ConcessionState.Active &&
					x.Exhibitor.PaymentConcession.Concession.Supplier.Login == SessionData.Login
				)
				.Select(x => new MobileExhibitorGetAllVisitorsResult
				{
					Id = x.Id,
					VisitorEntranceId = x.VisitorEntranceId,
					VisitorLogin = x.VisitorLogin,
					VisitorName = x.VisitorName,
					ExhibitorId = x.ExhibitorId,
					EventId = x.EventId,
					EventName = x.Event.Name
				});

			return new ResultBase<MobileExhibitorGetAllVisitorsResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
