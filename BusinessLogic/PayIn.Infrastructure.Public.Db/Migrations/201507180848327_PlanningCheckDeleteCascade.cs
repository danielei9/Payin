namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class PlanningCheckDeleteCascade : DbMigration
	{
		public override void Up()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlPlanningChecks", "PlanningId", "dbo.ControlPlannings");
			//DropIndex("dbo.ControlPlanningChecks", new[] { "PlanningId" });
			//Sql(
			//	"UPDATE CPC " +
			//	"SET PlanningId = CPI.PlanningId " +
			//	"FROM " +
			//		"dbo.ControlPlanningChecks CPC INNER JOIN " +
			//		"dbo.ControlPlanningItems CPI ON (CPC.Id = CPI.SinceId OR CPC.Id = CPI.UntilId)"
			//);
			//Sql(
			//	"DELETE dbo.ControlPlanningChecks " +
			//	"WHERE PlanningId IS NULL"
			//);
			//AlterColumn("dbo.ControlPlanningChecks", "PlanningId", c => c.Int(nullable: false));
			//CreateIndex("dbo.ControlPlanningChecks", "PlanningId");
			//AddForeignKey("dbo.ControlPlanningChecks", "PlanningId", "dbo.ControlPlannings", "Id", cascadeDelete: true);
			//// XAVI MIGRATION
		}

		public override void Down()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlPlanningChecks", "PlanningId", "dbo.ControlPlannings");
			//DropIndex("dbo.ControlPlanningChecks", new[] { "PlanningId" });
			//AlterColumn("dbo.ControlPlanningChecks", "PlanningId", c => c.Int());
			//CreateIndex("dbo.ControlPlanningChecks", "PlanningId");
			//AddForeignKey("dbo.ControlPlanningChecks", "PlanningId", "dbo.ControlPlannings", "Id");
			//// XAVI MIGRATION
		}
	}
}
