namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewControlPresence1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Name = c.String(),
                        Observation = c.String(),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            AddColumn("dbo.ControlPlannings", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPresences", "LatitudeWanted", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ControlPresences", "LongitudeWanted", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ControlPresences", "Observations", c => c.String());
            AddColumn("dbo.ControlPresences", "TagType", c => c.Int());
            AddColumn("dbo.ControlPresences", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPresences", "TagId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPresences", "ItemId", c => c.Int());
            AddColumn("dbo.ControlPresences", "PlanningId", c => c.Int());
            AddColumn("dbo.ServiceConcessions", "Interval", c => c.Time(nullable: false, precision: 7));
            CreateIndex("dbo.ControlPresences", "ConcessionId");
            CreateIndex("dbo.ControlPresences", "TagId");
            CreateIndex("dbo.ControlPresences", "ItemId");
            CreateIndex("dbo.ControlPresences", "PlanningId");
            CreateIndex("dbo.ControlPlannings", "ConcessionId");
            AddForeignKey("dbo.ControlPresences", "PlanningId", "dbo.ControlPlannings", "Id");
            AddForeignKey("dbo.ControlPlannings", "ConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.ControlPresences", "ConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.ControlPresences", "TagId", "dbo.ServiceTags", "Id");
            AddForeignKey("dbo.ControlPresences", "ItemId", "dbo.ControlItems", "Id");
            DropColumn("dbo.ControlPresences", "Tag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPresences", "Tag", c => c.String());
            DropForeignKey("dbo.ControlPresences", "ItemId", "dbo.ControlItems");
            DropForeignKey("dbo.ServiceTags", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ControlPresences", "TagId", "dbo.ServiceTags");
            DropForeignKey("dbo.ControlPresences", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ControlPlannings", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ControlPresences", "PlanningId", "dbo.ControlPlannings");
            DropIndex("dbo.ServiceTags", new[] { "ConcessionId" });
            DropIndex("dbo.ControlPlannings", new[] { "ConcessionId" });
            DropIndex("dbo.ControlPresences", new[] { "PlanningId" });
            DropIndex("dbo.ControlPresences", new[] { "ItemId" });
            DropIndex("dbo.ControlPresences", new[] { "TagId" });
            DropIndex("dbo.ControlPresences", new[] { "ConcessionId" });
            DropColumn("dbo.ServiceConcessions", "Interval");
            DropColumn("dbo.ControlPresences", "PlanningId");
            DropColumn("dbo.ControlPresences", "ItemId");
            DropColumn("dbo.ControlPresences", "TagId");
            DropColumn("dbo.ControlPresences", "ConcessionId");
            DropColumn("dbo.ControlPresences", "TagType");
            DropColumn("dbo.ControlPresences", "Observations");
            DropColumn("dbo.ControlPresences", "LongitudeWanted");
            DropColumn("dbo.ControlPresences", "LatitudeWanted");
            DropColumn("dbo.ControlPlannings", "ConcessionId");
            DropTable("dbo.ServiceTags");
            DropTable("dbo.ControlItems");
        }
    }
}
