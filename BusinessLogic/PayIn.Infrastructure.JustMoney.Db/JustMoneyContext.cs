using PayIn.Domain.JustMoney;
using System.Collections.Generic;
using System.Data.Entity;
using Xp.Infrastructure.Db;

namespace PayIn.Infrastructure.JustMoney.Db
{
	public class JustMoneyContext : SchemaContext<JustMoneyContext>
	{
		public DbSet<BankCardTransaction> BankCard { get; set; }
		public DbSet<Log> Log { get; set; }
		public DbSet<LogArgument> LogArgument { get; set; }
		public DbSet<LogResult> LogResult { get; set; }
		public DbSet<Option> Option { get; set; }
		public DbSet<PrepaidCard> PrepaidCard { get; set; }

		#region Constructors
		public JustMoneyContext()
			: base("PayInDb", "JustMoney")
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
			JustMoneyContext context = null;

			try
			{
				context = new JustMoneyContext();
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
		private static JustMoneyContext AddToContext<T>(JustMoneyContext context, T entity, int count, int commitCount, bool recreateContext)
			where T : class
		{
			context.Set<T>().Add(entity);

			if (count % commitCount == 0)
			{
				context.SaveChanges();

				if (recreateContext)
				{
					context.Dispose();

					context = new JustMoneyContext();
					context.Configuration.AutoDetectChangesEnabled = false;
				}
			}

			return context;
		}
		#endregion AddToContext
	}
}
