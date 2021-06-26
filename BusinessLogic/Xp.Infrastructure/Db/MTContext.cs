using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Infrastructure;
using System.Threading;

namespace Xp.Infrastructure.Db
{
	public abstract class MTContext<T> : SchemaContext<T>, IDbModelCacheKeyProvider
		where T : MTContext<T>
	{
		protected readonly string MigrationsNameSpace;

		private static Dictionary<string, string> UpdatedTenants = new Dictionary<string, string>();

		#region Constructors
		static MTContext()
		{
			Database.SetInitializer<MTContext<T>>(null);
		}
		public MTContext(string connectionString, string tenant, string migrationsNameSpace)
			: base(connectionString, tenant)
		{
			Database.SetInitializer<MTContext<T>>(null);
			MigrationsNameSpace = migrationsNameSpace;
		}
		#endregion Constructors

		#region Initialize
		public override void Initialize()
		{
			base.Initialize();

			if (Schema.IsNullOrEmpty())
				return;
			if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
				return;
			if (UpdatedTenants.ContainsKey(Schema))
				return;
			UpdatedTenants.Add(Schema, Schema);

			try
			{
				var tenantDataMigrationsConfiguration = new DbMigrationsConfiguration<T>();
				tenantDataMigrationsConfiguration.AutomaticMigrationsEnabled = false;
				tenantDataMigrationsConfiguration.AutomaticMigrationDataLossAllowed = true;
				tenantDataMigrationsConfiguration.SetSqlGenerator("System.Data.SqlClient", new MTMigrationSqlGenerator(Schema));
				tenantDataMigrationsConfiguration.SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, defaultSchema) => new HistoryContext(existingConnection, Schema));
				tenantDataMigrationsConfiguration.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(ConnectionString);
				tenantDataMigrationsConfiguration.MigrationsAssembly = typeof(T).Assembly;
				tenantDataMigrationsConfiguration.MigrationsNamespace = MigrationsNameSpace;
				tenantDataMigrationsConfiguration.ContextKey = Schema;

				var migrator = new DbMigrator(tenantDataMigrationsConfiguration);
				migrator.Update();
			}
			catch (AutomaticMigrationsDisabledException) { }
		}
		#endregion Initialize

		#region CacheKey
		public string CacheKey
		{
			get { return Schema; }
		}
		#endregion CacheKey
	}
}
