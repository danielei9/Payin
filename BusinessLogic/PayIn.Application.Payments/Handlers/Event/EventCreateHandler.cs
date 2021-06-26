using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
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
    [XpLog("Event", "Create")]
    [XpAnalytics("Event", "Create")]
    public class EventCreateHandler : IServiceBaseHandler<EventCreateArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventCreateArguments arguments)
		{
			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
                    (x.Id == arguments.PaymentConcessionId) &&
					(x.Concession.State == ConcessionState.Active) &&
					(
                        (x.Concession.Supplier.Login == SessionData.Login) ||
                        (x.PaymentWorkers
                            .Any(y => y.Login == SessionData.Login)
                        )
                    )
				)
				.FirstOrDefault();
            if (paymentConcession == null)
                throw new ArgumentNullException("paymentConcessionId");

            var events = new Event
            {
                Longitude = arguments.Longitude,
                Latitude = arguments.Latitude,
                Place = arguments.Place ?? "",
                Name = arguments.Name,
                Description = arguments.Description ?? "",
                Capacity = arguments.Capacity,
                EventEnd = arguments.EventEnd,
                EventStart = arguments.EventStart,
                CheckInStart = arguments.CheckInStart,
                CheckInEnd = arguments.CheckInEnd,
                PaymentConcessionId = paymentConcession.Id,
                PhotoUrl = "",
                State = EventState.Active,
                EntranceSystemId = arguments.EntranceSystemId,
                Code = arguments.Code,
                Conditions = arguments.Conditions,
                ShortDescription = arguments.ShortDescription,
				Visibility = arguments.Visibility,
				ProfileId = arguments.ProfileId,
                MapUrl ="",
				MaxEntrancesPerCard = arguments.MaxEntrancesPerCard ?? int.MaxValue,
				MaxAmountToSpend = arguments.MaxAmountToSpend
			};
			await Repository.AddAsync(events);
			return events;
		}
		#endregion ExecuteAsync
	}
}
