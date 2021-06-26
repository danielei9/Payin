namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RalationPlanningItemAndPlanningCheck : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ControlPlanningChecks", new[] { "PlanningId" });
            DropIndex("dbo.ControlPlanningChecks", new[] { "CheckPointId" });
            AddColumn("dbo.ControlPlanningItems", "SinceId", c => c.Int(nullable: false));
			AddColumn("dbo.ControlPlanningItems", "UntilId", c => c.Int(nullable: false));
			AlterColumn("dbo.ControlPlanningChecks", "PlanningId", c => c.Int());
			AlterColumn("dbo.ControlPlanningChecks", "CheckPointId", c => c.Int());

			AddColumn("dbo.ControlPlanningChecks", "PlanningItemIdAux", c => c.Int(nullable: true));

			Sql(
				"INSERT INTO dbo.ControlPlanningChecks (date, PlanningItemIdAux) " +
				"SELECT CPI.Since, CPI.Id " +
				"FROM dbo.ControlPlanningItems CPI "
			);
			Sql(
				"UPDATE CPI " +
				"SET SinceId = CPC.Id " +
				"FROM " +
					"dbo.ControlPlanningItems CPI INNER JOIN " +
					"dbo.ControlPlanningChecks CPC ON CPI.Id = CPC.PlanningItemIdAux "
			);
			Sql(
				"UPDATE ControlPlanningChecks " +
				"SET PlanningItemIdAux = NULL "
			);
			Sql(
				"INSERT INTO dbo.ControlPlanningChecks (date, PlanningItemIdAux) " +
				"SELECT CPI.Until, CPI.Id " +
				"FROM dbo.ControlPlanningItems CPI "
			);
			Sql(
				"UPDATE CPI " +
				"SET UntilId = CPC.Id " +
				"FROM " +
					"dbo.ControlPlanningItems CPI INNER JOIN " +
					"dbo.ControlPlanningChecks CPC ON CPI.Id = CPC.PlanningItemIdAux "
			);

			DropColumn("dbo.ControlPlanningChecks", "PlanningItemIdAux");

            CreateIndex("dbo.ControlPlanningChecks", "PlanningId");
            CreateIndex("dbo.ControlPlanningChecks", "CheckPointId");
            CreateIndex("dbo.ControlPlanningItems", "SinceId");
            CreateIndex("dbo.ControlPlanningItems", "UntilId");
            AddForeignKey("dbo.ControlPlanningItems", "SinceId", "dbo.ControlPlanningChecks", "Id");
            AddForeignKey("dbo.ControlPlanningItems", "UntilId", "dbo.ControlPlanningChecks", "Id");
            DropColumn("dbo.ControlPlanningItems", "Since");
            DropColumn("dbo.ControlPlanningItems", "Until");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPlanningItems", "Until", c => c.DateTime(nullable: false));
			AddColumn("dbo.ControlPlanningItems", "Since", c => c.DateTime(nullable: false));
			DropForeignKey("dbo.ControlPlanningItems", "UntilId", "dbo.ControlPlanningChecks");
			DropForeignKey("dbo.ControlPlanningItems", "SinceId", "dbo.ControlPlanningChecks");

			Sql(
				"UPDATE CPI " +
				"SET Since = CPC.Date " +
				"FROM " +
					"dbo.ControlPlanningItems CPI INNER JOIN " +
					"dbo.ControlPlanningChecks CPC ON CPI.SinceId = CPC.Id "
			);
			Sql(
				"UPDATE CPI " +
				"SET Until = CPC.Date " +
				"FROM " +
					"dbo.ControlPlanningItems CPI INNER JOIN " +
					"dbo.ControlPlanningChecks CPC ON CPI.UntilId = CPC.Id "
			);
			Sql(
				"DELETE FROM dbo.ControlPlanningChecks " +
				"WHERE EXISTS " +
					"(SELECT * " +
					"FROM dbo.ControlPlanningItems CPI " +
					"WHERE " +
						"dbo.ControlPlanningChecks.Id = CPI.SinceId AND " +
						"CPI.SinceId is not null " +
					") "
			);
			Sql(
				"DELETE FROM dbo.ControlPlanningChecks " +
				"WHERE EXISTS " +
					"(SELECT * " +
					"FROM dbo.ControlPlanningItems CPI " +
					"WHERE " +
						"dbo.ControlPlanningChecks.Id = CPI.UntilId AND " +
						"CPI.UntilId is not null "+
					") "
			);

            DropIndex("dbo.ControlPlanningItems", new[] { "UntilId" });
            DropIndex("dbo.ControlPlanningItems", new[] { "SinceId" });
            DropIndex("dbo.ControlPlanningChecks", new[] { "CheckPointId" });
            DropIndex("dbo.ControlPlanningChecks", new[] { "PlanningId" });
            AlterColumn("dbo.ControlPlanningChecks", "CheckPointId", c => c.Int(nullable: false));
            AlterColumn("dbo.ControlPlanningChecks", "PlanningId", c => c.Int(nullable: false));
            DropColumn("dbo.ControlPlanningItems", "UntilId");
            DropColumn("dbo.ControlPlanningItems", "SinceId");
            CreateIndex("dbo.ControlPlanningChecks", "CheckPointId");
            CreateIndex("dbo.ControlPlanningChecks", "PlanningId");
        }
    }
}
