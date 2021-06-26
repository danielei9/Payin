namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPresenceReestructuration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlPresences", "PlanningId", "dbo.ControlPlannings");
            DropIndex("dbo.ControlPresences", new[] { "PlanningId" });
            AddColumn("dbo.ControlPresences", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlPresences", "PlanningId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPresences", "PlanningId", c => c.Int());
            DropColumn("dbo.ControlPresences", "CreatedAt");
            CreateIndex("dbo.ControlPresences", "PlanningId");
            AddForeignKey("dbo.ControlPresences", "PlanningId", "dbo.ControlPlannings", "Id");
        }
    }
}
