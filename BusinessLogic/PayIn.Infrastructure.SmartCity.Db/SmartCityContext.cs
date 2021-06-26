using PayIn.Domain.SmartCity;
using System.Collections.Generic;
using System.Data.Entity;
using Xp.Infrastructure.Db;

namespace PayIn.Infrastructure.SmartCity.Db
{
	public class SmartCityContext : SchemaContext<SmartCityContext>
	{
		public DbSet<Alarm> Alarm { get; set; }
		public DbSet<Alert> Alert { get; set; }
		public DbSet<Component> Component { get; set; }
		public DbSet<Data> Data { get; set; }
		public DbSet<DataSet> DataSet { get; set; }
		public DbSet<Device> Device { get; set; }
		public DbSet<EnergyContract> EnergyContract { get; set; }
		public DbSet<EnergyHoliday> EnergyHoliday { get; set; }
		public DbSet<EnergyTariff> EnergyTariff { get; set; }
		public DbSet<EnergyTariffPeriod> EnergyTariffPeriod { get; set; }
		public DbSet<EnergyTariffPrice> EnergyTariffPrice { get; set; }
		public DbSet<EnergyTariffSchedule> EnergyTariffSchedule { get; set; }
		public DbSet<EnergyTariffTimeTable> EnergyTariffTimeTable { get; set; }
		public DbSet<Sensor> Sensor { get; set; }

		#region Constructors
		public SmartCityContext()
			: base("PayInDb", "SmartCity")
		{
		}
		#endregion Constructors

		#region OnModelCreating
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
		#endregion OnModelCreating

		#region BulkInsert
		public static void BulkInsert<T>(List<T> items)
			where T : class
		{
			SmartCityContext context = null;

			try
			{
				context = new SmartCityContext();
				context.Configuration.AutoDetectChangesEnabled = false;

				int count = 0;
				foreach (var entityToInsert in items)
				{
					++count;
					context = AddToContext(context, entityToInsert, count, 1000, true);
				}

				context.SaveChanges();
			}
			finally
			{
				if (context != null)
					context.Dispose();
			}
		}
		#endregion BulkInsert

		#region AddToContext
		private static SmartCityContext AddToContext<T>(SmartCityContext context, T entity, int count, int commitCount, bool recreateContext)
			where T : class
		{
			context.Set<T>().Add(entity);

			if (count % commitCount == 0)
			{
				context.SaveChanges();

				if (recreateContext)
				{
					context.Dispose();

					context = new SmartCityContext();
					context.Configuration.AutoDetectChangesEnabled = false;
				}
			}

			return context;
		}
		#endregion AddToContext
	}
}
