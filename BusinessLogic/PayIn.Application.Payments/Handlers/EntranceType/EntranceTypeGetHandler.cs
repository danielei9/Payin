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
	public class EntranceTypeGetHandler :
		IQueryBaseHandler<EntranceTypeGetArguments, EntranceTypeGetResult>
	{
		private readonly IEntityRepository<EntranceType> Repository;

		#region Constructors
		public EntranceTypeGetHandler(
			IEntityRepository<EntranceType> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeGetResult>> ExecuteAsync(EntranceTypeGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);
			
			var result = item
				.Select(x => new 
				{
					x.Id,
					x.IsVisible,
					x.Name,
                    x.Code,
					x.State,
					x.Description,
					x.Price,
					x.PhotoUrl,
					x.MaxEntrance,
					SellStart = (x.SellStart == XpDateTime.MinValue) ? null : (DateTime?)x.SellStart,
					SellEnd = (x.SellEnd == XpDateTime.MaxValue) ? null : (DateTime?)x.SellEnd,
					CheckInStart = (x.CheckInStart == XpDateTime.MinValue) ? null : (DateTime?)x.CheckInStart,
					CheckInEnd = (x.CheckInEnd == XpDateTime.MaxValue) ? null : (DateTime?)x.CheckInEnd,
					x.ExtraPrice,
					x.EventId,
					EventName = x.Event.Name,
					x.RangeMin,
					x.RangeMax,
                    x.Conditions,
                    x.MaxSendingCount,
                    x.ShortDescription,
					x.NumDay,
					x.StartDay,
					x.EndDay,
					x.Visibility
				})
				.ToList()
				.Select(x => new EntranceTypeGetResult
				{
					Id = x.Id,
					IsVisible = x.IsVisible,
					Name = x.Name,
                    Code = x.Code,
					State = x.State,
					Description = x.Description,
					Price = x.Price,
					PhotoUrl = x.PhotoUrl,
					MaxEntrance = x.MaxEntrance == int.MaxValue ? (int?)null : x.MaxEntrance,
					SellStart = x.SellStart.ToUTC(),
					SellEnd = x.SellEnd.ToUTC(),
					CheckInStart = x.CheckInStart.ToUTC(),
					CheckInEnd = x.CheckInEnd.ToUTC(),
					ExtraPrice = x.ExtraPrice,
					EventId = x.EventId,
					EventName = x.EventName,
					RangeMin = x.RangeMin,
					RangeMax = x.RangeMax,
                    Conditions = x.Conditions,
                    MaxSendingCount = x.MaxSendingCount,
                    ShortDescription = x.ShortDescription,
					NumDays = x.NumDay,
					StartDay = x.StartDay,
					EndDay = x.EndDay,
					Visibility = x.Visibility
				});
			return new ResultBase<EntranceTypeGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
