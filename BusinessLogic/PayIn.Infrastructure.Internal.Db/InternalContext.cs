using PayIn.Domain.Internal;
using System.Data.Entity;
using Xp.Infrastructure.Db;

namespace PayIn.Infrastructure.Internal.Db
{
	public class InternalContext : SchemaContext<InternalContext>
	{
		public DbSet<PaymentGateway> PaymentGateway { get; set; }
		public DbSet<PaymentMedia> PaymentMedia { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Log> Log { get; set; }
		public DbSet<LogArgument> LogArgument { get; set; }
		public DbSet<Option> Options { get; set; }

		#region Constructors
		public InternalContext()
			: base("PayInInternalDb", "internal")
		{
		}
		#endregion Constructors
	}
}
