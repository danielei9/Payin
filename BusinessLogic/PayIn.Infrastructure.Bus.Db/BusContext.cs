using PayIn.Domain.Bus;
using System.Collections.Generic;
using System.Data.Entity;
using Xp.Infrastructure.Db;

namespace PayIn.Infrastructure.Bus.Db
{
	public class BusContext : SchemaContext<BusContext>
	{
		public DbSet<Line> Line { get; set; }
		public DbSet<Link> Link { get; set; }
		public DbSet<Request> Request { get; set; }
		public DbSet<RequestStop> RequestStop { get; set; }
		public DbSet<Route> Route { get; set; }
		public DbSet<Stop> Stop { get; set; }
		public DbSet<TimeTable> TimeTable { get; set; }
		public DbSet<Vehicle> Vehicle { get; set; }

		#region Constructors
		public BusContext()
			: base("PayInDb", "Bus")
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
			BusContext context = null;

			try
			{
				context = new BusContext();
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
		private static BusContext AddToContext<T>(BusContext context, T entity, int count, int commitCount, bool recreateContext)
			where T : class
		{
			context.Set<T>().Add(entity);

			if (count % commitCount == 0)
			{
				context.SaveChanges();

				if (recreateContext)
				{
					context.Dispose();

					context = new BusContext();
					context.Configuration.AutoDetectChangesEnabled = false;
				}
			}

			return context;
		}
		#endregion AddToContext
	}
}
