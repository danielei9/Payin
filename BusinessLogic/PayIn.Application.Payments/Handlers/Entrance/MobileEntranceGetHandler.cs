using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEntranceGetHandler :
		IQueryBaseHandler<MobileEntranceGetArguments, MobileEntranceGetResult>
	{		
		private readonly IEntityRepository<Entrance> Repository;

		#region Constructors
		public MobileEntranceGetHandler(IEntityRepository<Entrance> repository) 
		{
			if (repository == null) throw new ArgumentNullException("repository");
						
			Repository = repository;			
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<MobileEntranceGetResult>> ExecuteAsync(MobileEntranceGetArguments arguments)
		{
			var now = DateTime.UtcNow;

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id)
				)
				.Select(x => new {
					Id = x.Id,
					Name = x.EntranceType.Name,
                    EntranceTypeConditions = x.EntranceType.Conditions,
                    EventConditions = x.Event.Conditions,
					Timestamp = x.Timestamp,
					Price = x.TicketLine == null ? 0 : x.TicketLine.Amount,
					TicketId = x.TicketLine == null ? (int?)null : x.TicketLine.TicketId,
					Description = x.EntranceType.Description,
					PhotoUrl = x.EntranceType.PhotoUrl,
                    EventPhotoUrl = x.EntranceType.Event.PhotoUrl,
                    Place = x.Event.Place,
					EventStart = x.Event.EventStart,
					EventEnd = x.Event.EventEnd,
					CheckInStart = x.Event.CheckInStart,
					CheckInEnd = x.Event.CheckInEnd,
					CodeTemplate = x.Event.EntranceSystem.Template,
					CodeTemplateText = x.Event.EntranceSystem.TemplateText,
					CodeType = x.Event.EntranceSystem.Type,
					UserName = x.UserName,
					LastName = x.LastName,
					UserTaxNumber = x.VatNumber,
					EventCode = x.Event.Code,
					EntranceTypeCode = "",
					EntranceCode = x.Code,
					EventMapUrl = x.Event.MapUrl,
					Activities = x.Event.Activities
						.Select(y => new
						{
							y.Id,
							y.Name,
							y.Description,
							y.Start,
							y.End
						})
				})
				.ToList()
				.Select(x => new MobileEntranceGetResult
				{
					Id = x.Id,
					Name = x.Name,
                    EntranceTypeConditions = x.EntranceTypeConditions,
                    EventConditions = x.EventConditions,
                    Timestamp = x.Timestamp.ToUTC(),
					Price = x.Price,
					TicketId = x.TicketId,
					Description = x.Description,
					PhotoUrl = x.PhotoUrl,
                    EventPhotoUrl = x.EventPhotoUrl,
                    Place = x.Place,
					EventStart = x.EventStart.ToUTC(),
					EventEnd = x.EventEnd.ToUTC(),
					CheckInStart = x.CheckInStart.ToUTC(),
					CheckInEnd = x.CheckInEnd.ToUTC(),
					UserName = x.UserName,
					LastName = x.LastName,
					UserTaxNumber = x.UserTaxNumber,
                    Code = x.CodeTemplate.FormatString(x.EventCode ?? 0, x.EntranceTypeCode, x.EntranceCode),
                    CodeText = x.CodeTemplateText.FormatString(x.EventCode ?? 0, x.EntranceTypeCode, x.EntranceCode),
					CodeType = x.CodeType,
					MapUrl = x.EventMapUrl,
					Activities = x.Activities
						.Select(y => new MobileEntranceGetResult_Activities
						{
							Id = y.Id,
							Name = y.Name,
							Description = y.Description,
							Start = y.Start.ToUTC(),
							End = y.End.ToUTC()
						})
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;
			return new ResultBase<MobileEntranceGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
