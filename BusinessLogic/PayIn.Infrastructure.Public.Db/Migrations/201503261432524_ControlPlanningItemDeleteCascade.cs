namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPlanningItemDeleteCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlPlanningItems", "PlanningId", "dbo.ControlPlannings");
            AddForeignKey("dbo.ControlPlanningItems", "PlanningId", "dbo.ControlPlannings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlPlanningItems", "PlanningId", "dbo.ControlPlannings");
            AddForeignKey("dbo.ControlPlanningItems", "PlanningId", "dbo.ControlPlannings", "Id");
        }
    }
}
