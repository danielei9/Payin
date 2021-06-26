using Microsoft.AspNet.Identity.EntityFramework;
using PayIn.Domain.Security;
using System.Data.Entity;

namespace PayIn.Infrastructure.Security.Db
{
	public class AuthContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Client> Clients { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		#region Constructors
		public AuthContext()
			: base("SecurityDb")
		{
			Database.CommandTimeout = 300000;
		}
		#endregion Constructors

		#region OnModelCreating
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("security");

			base.OnModelCreating(modelBuilder);
		}
		#endregion OnModelCreating
	}
}