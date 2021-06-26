using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;

namespace Xp.Application
{
	public static class PositionManager
	{
		#region Calculate
		public static IEnumerable<TItem> Calculate<TItem>(ITrace<TItem> result)
			where TItem : IPosition
		{
			var items = Activator.CreateInstance<List<TItem>>();

			TItem posPrevious = default(TItem);
			TItem posPrevious2 = default(TItem);
			foreach (var position in result.Items)
			{
				if ((posPrevious != null) && ((position.Date.Value - posPrevious.Date.Value).TotalSeconds == 0))
					continue;
				
				if (posPrevious != null)
					CalculateValues(position, posPrevious);

				if (posPrevious != null && posPrevious.Acceleration > 2m)
				{
					items.Remove(posPrevious);
					CalculateValues(position, posPrevious2);
				}
				else if (position.Acceleration > 2m && posPrevious != null && posPrevious.Distance == 0)
				{
					items.Remove(posPrevious);
					CalculateValues(position, posPrevious2);
				}
				else
					posPrevious2 = posPrevious;

				posPrevious = position;
				items.Add(position);
			}

			return items;
		}
		#endregion Calculate

		#region CalculateValues
		private static void CalculateValues(IPosition position, IPosition posPrevious)
		{
			position.Distance = XpMath.Distance(
				Convert.ToDouble(posPrevious.Latitude),
				Convert.ToDouble(posPrevious.Longitude),
				Convert.ToDouble(position.Latitude),
				Convert.ToDouble(position.Longitude)
			);
			position.Elapsed = position.Date.Value - posPrevious.Date.Value;
			position.Velocity = Math.Round(
				Convert.ToDouble(position.Distance) / Convert.ToDouble(position.Elapsed.Value.TotalSeconds),
				6
			).ToDecimal();
			position.Acceleration = Math.Round(
				Convert.ToDouble(position.Velocity - posPrevious.Velocity) / Convert.ToDouble(position.Elapsed.Value.TotalSeconds),
				6
			).ToDecimal();
		}
		#endregion CalculateValues
	}
}
