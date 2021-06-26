namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReestructurandoTodoElBusModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Bus.Graphs", newName: "Routes");
            RenameTable(name: "Bus.Nodes", newName: "Stops");
            RenameColumn(table: "Bus.Links", name: "GraphId", newName: "RouteId");
            RenameIndex(table: "Bus.Links", name: "IX_GraphId", newName: "IX_RouteId");
            CreateTable(
                "Bus.Buses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Sense = c.Int(nullable: false),
                        LineId = c.Int(),
                        CurrentStopId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Stops", t => t.CurrentStopId)
                .ForeignKey("Bus.Lines", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.CurrentStopId);
            
            CreateTable(
                "Bus.RequestStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Stops", t => t.StopId)
                .Index(t => t.StopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.Buses", "LineId", "Bus.Lines");
            DropForeignKey("Bus.RequestStops", "StopId", "Bus.Stops");
            DropForeignKey("Bus.Buses", "CurrentStopId", "Bus.Stops");
            DropIndex("Bus.RequestStops", new[] { "StopId" });
            DropIndex("Bus.Buses", new[] { "CurrentStopId" });
            DropIndex("Bus.Buses", new[] { "LineId" });
            DropTable("Bus.RequestStops");
            DropTable("Bus.Buses");
            RenameIndex(table: "Bus.Links", name: "IX_RouteId", newName: "IX_GraphId");
            RenameColumn(table: "Bus.Links", name: "RouteId", newName: "GraphId");
            RenameTable(name: "Bus.Stops", newName: "Nodes");
            RenameTable(name: "Bus.Routes", newName: "Graphs");
        }
    }
}
