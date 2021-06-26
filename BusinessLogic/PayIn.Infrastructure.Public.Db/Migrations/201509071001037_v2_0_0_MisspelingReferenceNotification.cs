namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v2_0_0_MisspelingReferenceNotification : DbMigration
	{
		public override void Up()
		{
			RenameColumn("dbo.ServiceNotifications", "ReferrenceId", "ReferenceId");
			RenameColumn("dbo.ServiceNotifications", "ReferrenceClass", "ReferenceClass");
		}

		public override void Down()
		{
			RenameColumn("dbo.ServiceNotifications", "ReferenceId", "ReferrenceId");
			RenameColumn("dbo.ServiceNotifications", "ReferenceClass", "ReferrenceClass");
		}
	}
}
