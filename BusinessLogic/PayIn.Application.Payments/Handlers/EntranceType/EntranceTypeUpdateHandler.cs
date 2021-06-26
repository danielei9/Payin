using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeUpdateHandler :
		IServiceBaseHandler<EntranceTypeUpdateArguments>
	{
		private readonly IEntityRepository<EntranceType> Repository;

		#region Constructors
		public EntranceTypeUpdateHandler(
			IEntityRepository<EntranceType> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeUpdateArguments arguments)
		{
			if ((arguments.RangeMin == null) && (arguments.RangeMax != null))
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The minimun range can't be null if maximun range has value");

			if (arguments.RangeMin > arguments.RangeMax)
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The minimun range can't be greater than maximun range");

			if ((arguments.RangeMin < 0) || (arguments.RangeMax < 0))
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. Negative ranges not allowed");

			if ((arguments.RangeMax - arguments.RangeMin) < arguments.MaxEntrance)
				throw new Exception("ERROR, the introduced ranges are not allowed. Please, introduce the ranges again. The ranges must contemplate the maximun entrances");

			var item = (await Repository.GetAsync(arguments.Id));

			item.IsVisible = arguments.IsVisible;
			item.Name = arguments.Name;
            item.Code = arguments.Code;
			item.State = arguments.State;
			item.Visibility = arguments.Visibility;
			item.Description = arguments.Description;
            item.ShortDescription = arguments.ShortDescription;
            item.Conditions = arguments.Conditions;
            item.Price = arguments.Price;
			item.MaxEntrance = arguments.MaxEntrance ?? int.MaxValue;
			item.SellStart = arguments.SellStart;
			item.SellEnd = arguments.SellEnd;
			item.CheckInStart = arguments.CheckInStart;
			item.CheckInEnd = arguments.CheckInEnd;
			item.ExtraPrice = arguments.ExtraPrice;
			item.RangeMin = arguments.RangeMin;
			item.RangeMax = arguments.RangeMax;
            item.MaxSendingCount = arguments.MaxSendingCount;
			item.NumDay = arguments.NumDays;
			item.StartDay = arguments.StartDay;
			item.EndDay = arguments.EndDay;

			return item;
		}
		#endregion ExecuteAsync
	}
}
