using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Xp.Domain;

namespace Xp.Infrastructure.Db
{
	public abstract class SchemaContext<T> : DbContext
		where T : SchemaContext<T>
	{
		protected readonly string ConnectionString;
		protected readonly string Schema;

		#region Constructors
		public SchemaContext(string connectionString, string tenant)
			: base(connectionString)
		{
			ConnectionString = connectionString;
			Schema = GetSchema(tenant);
		}
		#endregion Constructors

		#region Initialize
		public virtual void Initialize()
		{
		}
		#endregion Initialize

		#region OnModelCreating
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//Mirar: http://msdn.microsoft.com/en-us/data/jj591617.aspx
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			if (!Schema.IsNullOrEmpty())
				modelBuilder.HasDefaultSchema(Schema);

			PrecisionAttribute.ConfigureModelBuilder(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}
		#endregion OnModelCreating

		#region GetSchema
		protected string GetSchema(string tenant)
		{
			if (tenant.IsNullOrEmpty())
				return tenant;

			return tenant
				.Replace(" ", "");
		}
		#endregion GetSchema
	}
}
