using System;
using System.Collections.Generic;
using System.Linq;

namespace PayIn.Application.Dto
{
	public class CalculateZone
	{
		public int															Id							{ get; set; }
		public string														Name						{ get; set; }
		public int															SupplierId			{ get; set; }
		public string														SupplierName		{ get; set; }
		public int															ConcessionId		{ get; set; }
		public string														ConcessionName	{ get; set; }
		public IEnumerable<CalculatePrice>			Prices					{ get; set; }
		public IEnumerable<CalculateTimeTable>	TimeTable				{ get; set; }

		#region Constructors
		public CalculateZone()
		{
			Prices = new List<CalculatePrice>();
			TimeTable = new List<CalculateTimeTable>();
		}
		#endregion Constructors

		#region GetHashCode
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		#endregion GetHashCode

		#region Equals
		public override bool Equals(object obj)
		{
			var item = obj as CalculateZone;
			if (item == null)
				return false;

			return Id == item.Id;
		}
		#endregion Equals

		#region GetAmount
		public decimal GetAmount(decimal amount)
		{
			var max = this.Prices
				.Max(y => y.Price);
			if (amount >= max)
				return max;

			var min = this.Prices
				.Min(y => y.Price);
			if (amount <= min)
				return min;

			return amount;
		}
		public decimal GetAmount(TimeSpan elapsedTime)
		{
			var previous = this.Prices
				.Where(x => x.Time <= elapsedTime)
				.OrderByDescending(x => x.Time)
				.FirstOrDefault();
			var next = this.Prices
				.Where(y => y.Time >= elapsedTime)
				.OrderBy(x => x.Time)
				.FirstOrDefault();

			if (previous == null)
				return next.Price;
			if (next == null)
				return previous.Price;

			if (previous.Time == elapsedTime)
				return previous.Price;
			if (next.Time == elapsedTime)
				return next.Price;

			var pendiente = (next.Price - previous.Price) / (next.Time - previous.Time).TotalMinutes.ToDecimal();
			var base_ = next.Price - pendiente * next.Time.TotalMinutes.ToDecimal();

			var result = base_ + pendiente * elapsedTime.TotalMinutes.ToDecimal();
			return result;
		}
		#endregion GetAmount

		#region GetElapsedTime
		public TimeSpan GetElapsedTime(TimeSpan elapsedTime)
		{
			var max = this.Prices
				.Max(y => y.Time);
			if (elapsedTime >= max)
				return max;

			var min = this.Prices
				.Min(y => y.Time);
			if (elapsedTime <= min)
				return min;

			return elapsedTime;
		}
		public TimeSpan GetElapsedTime(decimal amount)
		{
			var previous = this.Prices
				.Where(x => x.Price <= amount)
				.OrderByDescending(x => x.Price)
				.FirstOrDefault();
			var next = this.Prices
				.Where(y => y.Price >= amount)
				.OrderBy(x => x.Price)
				.FirstOrDefault();

			if (previous == null)
				return next.Time;
			if (next == null)
				return previous.Time;

			if (previous.Price == amount)
				return previous.Time;
			if (next.Price == amount)
				return next.Time;

			var pendiente = (next.Time - previous.Time).TotalMinutes.ToDecimal() / (next.Price - previous.Price);
			var base_ = next.Time - TimeSpan.FromMinutes((pendiente * next.Price).ToDouble());

			var result = base_ + TimeSpan.FromMinutes((pendiente * amount).ToDouble());
			return result;
		}
		#endregion GetElapsedTime

		#region GetUntil
		public DateTime GetUntil(DateTime? datetime, decimal amount)
		{
			var minutes = Convert.ToInt32(this.GetElapsedTime(amount).TotalMinutes);
			return this.GetUntil(datetime ?? DateTime.Now, minutes);
		}
		private DateTime GetUntil(DateTime datetime, int minutes)
		{
			if (minutes == 0)
				return datetime;

			var time = datetime.TimeOfDay.TruncateMinute();
			var dayOfWeek = datetime.DayOfWeek;

			var timeTable = this.TimeTable
				.Where(x =>
					(x.FromWeekday <= dayOfWeek) &&
					(x.UntilWeekday >= dayOfWeek) &&
					(x.FromHour.Add(x.DurationHour) >= time)
				)
				.OrderBy(x => x.FromHour)
				.FirstOrDefault();

			if (timeTable != null)
			{
				var start = timeTable.FromHour >= time ? timeTable.FromHour : time;
				var rest = Math.Min(
					timeTable.DurationHour.Hours * 60 + timeTable.DurationHour.Minutes,
					minutes);

				return this.GetUntil(datetime.Date.Add(start).Add(TimeSpan.FromMinutes(rest)), minutes - rest);
			}

			return this.GetUntil(datetime.Date.AddDays(1), minutes);
		}
		#endregion GetUntil
	}
}
