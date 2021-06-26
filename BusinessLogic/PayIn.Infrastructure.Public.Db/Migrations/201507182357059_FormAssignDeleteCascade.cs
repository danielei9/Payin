namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class FormAssignDeleteCascade : DbMigration
	{
		public override void Up()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlFormAssigns", "CheckId", "dbo.ControlPlanningChecks");
			//DropIndex("dbo.ControlFormAssigns", new[] { "CheckId" });
			//AlterColumn("dbo.ControlFormAssigns", "CheckId", c => c.Int(nullable: false));
			//CreateIndex("dbo.ControlFormAssigns", "CheckId");
			//AddForeignKey("dbo.ControlFormAssigns", "CheckId", "dbo.ControlPlanningChecks", "Id", cascadeDelete: true);
			//// XAVI MIGRATION
		}

		public override void Down()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlFormAssigns", "CheckId", "dbo.ControlPlanningChecks");
			//DropIndex("dbo.ControlFormAssigns", new[] { "CheckId" });
			//AlterColumn("dbo.ControlFormAssigns", "CheckId", c => c.Int());
			//CreateIndex("dbo.ControlFormAssigns", "CheckId");
			//AddForeignKey("dbo.ControlFormAssigns", "CheckId", "dbo.ControlPlanningChecks", "Id");
			//// XAVI MIGRATION
		}
	}
}
