namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanningReestructuration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlPlanningItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        PlanningId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlPlannings", t => t.PlanningId)
                .Index(t => t.PlanningId);
            
            AddColumn("dbo.ControlPlannings", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "CheckDuration", c => c.Time(precision: 7));
            AddColumn("dbo.ControlPlannings", "WorkerId", c => c.Int(nullable: false));
						Sql(
							"UPDATE dbo.ControlPlannings " +
							"SET WorkerId = SW.id " +
							"FROM " +
								"dbo.ControlPlannings CP INNER JOIN " +
								"dbo.ServiceWorkers SW ON CP.[login] = SW.[login]"
						);
						Sql(
							"DELETE dbo.ControlPlannings " +
							"WHERE WorkerId = 0"
						);
            CreateIndex("dbo.ControlPlannings", "WorkerId");
            AddForeignKey("dbo.ControlPlannings", "WorkerId", "dbo.ServiceWorkers", "Id");
            DropColumn("dbo.ControlPlannings", "Login");
            DropColumn("dbo.ControlPlannings", "Since");
            DropColumn("dbo.ControlPlannings", "Until");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPlannings", "Until", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "Since", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "Login", c => c.String());
            DropForeignKey("dbo.ControlPlannings", "WorkerId", "dbo.ServiceWorkers");
            DropForeignKey("dbo.ControlPlanningItems", "PlanningId", "dbo.ControlPlannings");
            DropIndex("dbo.ControlPlanningItems", new[] { "PlanningId" });
            DropIndex("dbo.ControlPlannings", new[] { "WorkerId" });
            DropColumn("dbo.ControlPlannings", "WorkerId");
            DropColumn("dbo.ControlPlannings", "CheckDuration");
            DropColumn("dbo.ControlPlannings", "Date");
            DropTable("dbo.ControlPlanningItems");
        }
    }
}
