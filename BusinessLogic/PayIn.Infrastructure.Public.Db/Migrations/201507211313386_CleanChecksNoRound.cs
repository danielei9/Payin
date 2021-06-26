namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class CleanChecksNoRound : DbMigration
	{
		public override void Up()
		{
			//// XAVI MIGRATION
			//Sql(
			//	"UPDATE CP SET CP.PlanningCheckId = null " +
			//	"FROM " +
			//		"dbo.ControlPresences CP INNER JOIN " +
			//		"[dbo].[ControlPlanningChecks] CPC ON CP.PlanningCheckId = CPC.Id INNER JOIN " +
			//		"[dbo].[ServiceCheckPoints] SCP ON CPC.CheckPointId = SCP.Id " +
			//	"WHERE " +
			//		"CPC.CheckPointId IS NOT NULL AND " +
			//		"SCP.Type != 4"
			//	);
			//Sql(
			//	"DELETE CPC " +
			//	"FROM " +
			//		"[dbo].[ControlPlanningChecks] CPC INNER JOIN " +
			//		"[dbo].[ServiceCheckPoints] SCP ON CPC.CheckPointId = SCP.Id " +
			//	"WHERE " +
			//		"CPC.CheckPointId IS NOT NULL AND " +
			//		"SCP.Type != 4"
			//);
			//Sql(
			//	"DELETE CTC " +
			//	"FROM " +
			//		"[dbo].[ControlTemplateChecks] CTC INNER JOIN " +
			//		"[dbo].[ServiceCheckPoints] SCP ON CTC.CheckPointId = SCP.Id " +
			//	"WHERE " +
			//		"CTC.CheckPointId IS NOT NULL AND " +
			//		"SCP.Type != 4"
			//);
			//// XAVI MIGRATION
		}

		public override void Down()
		{
		}
	}
}
