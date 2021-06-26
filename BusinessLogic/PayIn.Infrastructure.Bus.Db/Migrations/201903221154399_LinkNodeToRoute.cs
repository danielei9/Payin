namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkNodeToRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.Stops", "RouteId", c => c.Int(nullable: false));
            CreateIndex("Bus.Stops", "RouteId");
            AddForeignKey("Bus.Stops", "RouteId", "Bus.Routes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.Stops", "RouteId", "Bus.Routes");
            DropIndex("Bus.Stops", new[] { "RouteId" });
            DropColumn("Bus.Stops", "RouteId");
        }
    }
}
