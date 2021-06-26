using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Common
{
	public class XpMath
	{
		#region Distance
		public static decimal Distance(double latitude1, double longitude1, double latitude2, double longitude2)
		{
			var difLatitud = (latitude2 - latitude1).FromRadians();
			var difLongitud = (longitude2 - longitude1).FromRadians();
			var earthRadio = 6378000;

			var a = 
				Math.Pow(Math.Sin(difLatitud / 2), 2) +
				Math.Cos(latitude1.FromRadians()) *
				Math.Cos(latitude2.FromRadians()) *
				Math.Pow(Math.Sin(difLongitud / 2), 2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return Math.Round(c * earthRadio, 6).ToDecimal();
		}
		#endregion Distance
	}
}
