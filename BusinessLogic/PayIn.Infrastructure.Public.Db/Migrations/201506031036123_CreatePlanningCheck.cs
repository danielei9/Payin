namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePlanningCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlPlanningChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        PlanningId = c.Int(nullable: false),
                        CheckPointId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlPlannings", t => t.PlanningId)
                .ForeignKey("dbo.ServiceCheckPoints", t => t.CheckPointId)
                .Index(t => t.PlanningId)
                .Index(t => t.CheckPointId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlPlanningChecks", "CheckPointId", "dbo.ServiceCheckPoints");
            DropForeignKey("dbo.ControlPlanningChecks", "PlanningId", "dbo.ControlPlannings");
            DropIndex("dbo.ControlPlanningChecks", new[] { "CheckPointId" });
            DropIndex("dbo.ControlPlanningChecks", new[] { "PlanningId" });
            DropTable("dbo.ControlPlanningChecks");
        }
    }
}
