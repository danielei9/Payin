using PayIn.Application.Dto.Payments.Arguments;
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
	public class EntranceTypeCreateHandler : 
		IServiceBaseHandler<EntranceTypeCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<EntranceType> Repository;
		private readonly IEntityRepository<Event> EventRepository;

		#region Constructors
		public EntranceTypeCreateHandler(
			ISessionData sessionData,
			IEntityRepository<EntranceType> repository,
			IEntityRepository<Event> eventRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (eventRepository == null) throw new ArgumentNullException("eventRepository");

			SessionData = sessionData;
			Repository = repository;
			EventRepository = eventRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeCreateArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.EventId == arguments.EventId);

			var entrancetypes = new EntranceType
			{
				Name = arguments.Name,
                Code = arguments.Code,
                Description = arguments.Description ?? "",
				Price = arguments.Price,
				PhotoUrl = "",
				MaxEntrance = arguments.MaxEntrance ?? int.MaxValue,
				SellStart = arguments.SellStart,
				SellEnd = arguments.SellEnd,
				CheckInStart = arguments.CheckInStart,
				CheckInEnd = arguments.CheckInEnd,
				ExtraPrice = (arguments.ExtraPrice == null ? 0 : (decimal)arguments.ExtraPrice),
				State = EntranceTypeState.Active,
				Visibility = arguments.Visibility,
				EventId = arguments.EventId,
				RangeMin = arguments.RangeMin,
				RangeMax = arguments.RangeMax,
                ShortDescription = arguments.ShortDescription ?? "",
                Conditions = arguments.Conditions ?? "",
                MaxSendingCount = arguments.MaxSendingCount,
				NumDay = arguments.NumDays,
				StartDay = arguments.StartDay,
				EndDay = arguments.EndDay,
				IsVisible = true
            };

			if ((arguments.RangeMin == null) && (arguments.RangeMax != null))
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The minimun range can't be null if maximun range has value");

			if (arguments.RangeMin > arguments.RangeMax)
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The minimun range can't be greater than maximun range");

			if ((arguments.RangeMin < 0) || (arguments.RangeMax < 0))
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. Negative ranges not allowed");

			if ((arguments.RangeMax - arguments.RangeMin) < arguments.MaxEntrance)
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The ranges must contemplate the maximun entrances");

			await Repository.AddAsync(entrancetypes);
			return entrancetypes;
		}
		#endregion ExecuteAsync
	}
}
