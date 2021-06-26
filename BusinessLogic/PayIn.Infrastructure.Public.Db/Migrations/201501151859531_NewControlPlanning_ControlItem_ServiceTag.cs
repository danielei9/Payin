namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewControlPlanning_ControlItem_ServiceTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlPlannings", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ControlPresences", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceTags", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ControlPresences", new[] { "ConcessionId" });
            DropIndex("dbo.ControlPresences", new[] { "ItemId" });
            DropIndex("dbo.ControlPlannings", new[] { "ConcessionId" });
            DropIndex("dbo.ServiceTags", new[] { "ConcessionId" });
            AddColumn("dbo.ControlItems", "Name", c => c.String());
            AddColumn("dbo.ControlItems", "Observation", c => c.String());
            AddColumn("dbo.ControlItems", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPlannings", "Since", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "Until", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "ItemId", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceTags", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ServiceTags", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ServiceTags", "TagType", c => c.Int());
            AddColumn("dbo.ServiceTags", "ItemId", c => c.Int(nullable: false));
            AlterColumn("dbo.ControlPresences", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlItems", "ConcessionId");
            CreateIndex("dbo.ControlPlannings", "ItemId");
            CreateIndex("dbo.ControlPresences", "ItemId");
            CreateIndex("dbo.ServiceTags", "ItemId");
            AddForeignKey("dbo.ControlItems", "ConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.ControlPlannings", "ItemId", "dbo.ControlItems", "Id");
            AddForeignKey("dbo.ServiceTags", "ItemId", "dbo.ControlItems", "Id");
            DropColumn("dbo.ControlPresences", "ConcessionId");
            DropColumn("dbo.ControlPlannings", "Tag");
            DropColumn("dbo.ControlPlannings", "Date");
            DropColumn("dbo.ControlPlannings", "Latitude");
            DropColumn("dbo.ControlPlannings", "Longitude");
            DropColumn("dbo.ControlPlannings", "ConcessionId");
            DropColumn("dbo.ServiceTags", "ConcessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceTags", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPlannings", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPlannings", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ControlPlannings", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ControlPlannings", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlPlannings", "Tag", c => c.String());
            AddColumn("dbo.ControlPresences", "ConcessionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ServiceTags", "ItemId", "dbo.ControlItems");
            DropForeignKey("dbo.ControlPlannings", "ItemId", "dbo.ControlItems");
            DropForeignKey("dbo.ControlItems", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ServiceTags", new[] { "ItemId" });
            DropIndex("dbo.ControlPresences", new[] { "ItemId" });
            DropIndex("dbo.ControlPlannings", new[] { "ItemId" });
            DropIndex("dbo.ControlItems", new[] { "ConcessionId" });
            AlterColumn("dbo.ControlPresences", "ItemId", c => c.Int());
            DropColumn("dbo.ServiceTags", "ItemId");
            DropColumn("dbo.ServiceTags", "TagType");
            DropColumn("dbo.ServiceTags", "Longitude");
            DropColumn("dbo.ServiceTags", "Latitude");
            DropColumn("dbo.ControlPlannings", "ItemId");
            DropColumn("dbo.ControlPlannings", "Until");
            DropColumn("dbo.ControlPlannings", "Since");
            DropColumn("dbo.ControlItems", "ConcessionId");
            DropColumn("dbo.ControlItems", "Observation");
            DropColumn("dbo.ControlItems", "Name");
            CreateIndex("dbo.ServiceTags", "ConcessionId");
            CreateIndex("dbo.ControlPlannings", "ConcessionId");
            CreateIndex("dbo.ControlPresences", "ItemId");
            CreateIndex("dbo.ControlPresences", "ConcessionId");
            AddForeignKey("dbo.ServiceTags", "ConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.ControlPresences", "ConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.ControlPlannings", "ConcessionId", "dbo.ServiceConcessions", "Id");
        }
    }
}
