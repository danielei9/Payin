namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteChangeRelatedToLine : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Bus.Stops", "RouteId", "Bus.Routes");
            DropIndex("Bus.Stops", new[] { "RouteId" });
			RenameColumn("Bus.Stops", "RouteId", "LineId");
            CreateIndex("Bus.Stops", "LineId");
            AddForeignKey("Bus.Stops", "LineId", "Bus.Lines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.Stops", "LineId", "Bus.Lines");
            DropIndex("Bus.Stops", new[] { "LineId" });
			RenameColumn("Bus.Stops", "LineId", "RouteId");
            CreateIndex("Bus.Stops", "RouteId");
            AddForeignKey("Bus.Stops", "RouteId", "Bus.Routes", "Id");
        }
    }
}
