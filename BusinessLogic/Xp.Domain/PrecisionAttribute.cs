using System;
using System.Data.Entity;
using System.Linq;

namespace Xp.Domain
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class PrecisionAttribute : Attribute
	{
		public byte Precision { get; set; }
		public byte Scale     { get; set; }

		#region Constructors
		public PrecisionAttribute(byte precision, byte scale)
		{
			Precision = precision;
			Scale = scale;
		}
		#endregion Constructors

		#region ConfigureModelBuilder
		public static void ConfigureModelBuilder(DbModelBuilder modelBuilder)
		{
			modelBuilder.Properties()
				.Where(x => 
					x
						.GetCustomAttributes(typeof(PrecisionAttribute), false)
						.Any())
				.Configure(c => 
					c.HasPrecision(
						c.ClrPropertyInfo.GetCustomAttributes(typeof(PrecisionAttribute), false).Cast<PrecisionAttribute>().First().Precision, 
						c.ClrPropertyInfo.GetCustomAttributes(typeof(PrecisionAttribute), false).Cast<PrecisionAttribute>().First().Scale
					));
			var xx = 1;
			xx++;
		}
		#endregion ConfigureModelBuilder
	}
}
