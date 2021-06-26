namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Bus.Lines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Bus.Graphs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sense = c.Int(nullable: false),
                        LineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Lines", t => t.LineId)
                .Index(t => t.LineId);
            
            CreateTable(
                "Bus.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Weight = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        GraphId = c.Int(nullable: false),
                        FromId = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Nodes", t => t.ToId)
                .ForeignKey("Bus.Nodes", t => t.FromId)
                .ForeignKey("Bus.Graphs", t => t.GraphId)
                .Index(t => t.GraphId)
                .Index(t => t.FromId)
                .Index(t => t.ToId);
            
            CreateTable(
                "Bus.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        Location = c.String(nullable: false),
                        Longitude = c.Decimal(precision: 18, scale: 2),
                        Latitude = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Bus.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        FromId = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Nodes", t => t.FromId)
                .ForeignKey("Bus.Nodes", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.Graphs", "LineId", "Bus.Lines");
            DropForeignKey("Bus.Links", "GraphId", "Bus.Graphs");
            DropForeignKey("Bus.Requests", "ToId", "Bus.Nodes");
            DropForeignKey("Bus.Requests", "FromId", "Bus.Nodes");
            DropForeignKey("Bus.Links", "FromId", "Bus.Nodes");
            DropForeignKey("Bus.Links", "ToId", "Bus.Nodes");
            DropIndex("Bus.Requests", new[] { "ToId" });
            DropIndex("Bus.Requests", new[] { "FromId" });
            DropIndex("Bus.Links", new[] { "ToId" });
            DropIndex("Bus.Links", new[] { "FromId" });
            DropIndex("Bus.Links", new[] { "GraphId" });
            DropIndex("Bus.Graphs", new[] { "LineId" });
            DropTable("Bus.Requests");
            DropTable("Bus.Nodes");
            DropTable("Bus.Links");
            DropTable("Bus.Graphs");
            DropTable("Bus.Lines");
        }
    }
}
