namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPresence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlPresences", "PlanningItemId", c => c.Int());
            AddColumn("dbo.ControlPresences", "PlanningCheckId", c => c.Int());
            CreateIndex("dbo.ControlPresences", "PlanningItemId");
            CreateIndex("dbo.ControlPresences", "PlanningCheckId");
            AddForeignKey("dbo.ControlPresences", "PlanningItemId", "dbo.ControlPlanningItems", "Id");
            AddForeignKey("dbo.ControlPresences", "PlanningCheckId", "dbo.ControlPlanningChecks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlPresences", "PlanningCheckId", "dbo.ControlPlanningChecks");
            DropForeignKey("dbo.ControlPresences", "PlanningItemId", "dbo.ControlPlanningItems");
            DropIndex("dbo.ControlPresences", new[] { "PlanningCheckId" });
            DropIndex("dbo.ControlPresences", new[] { "PlanningItemId" });
            DropColumn("dbo.ControlPresences", "PlanningCheckId");
            DropColumn("dbo.ControlPresences", "PlanningItemId");
        }
    }
}
