namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolucionandoError1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Bus.Requests", "FromId", "Bus.RequestStops");
            DropForeignKey("Bus.Requests", "ToId", "Bus.RequestStops");
			DropForeignKey("Bus.Requests", "FromId", "Bus.Nodes"); // A mano FK_Bus.Requests_Bus.Nodes_FromId
			DropForeignKey("Bus.Requests", "ToId", "Bus.Nodes"); // A mano FK_Bus.Requests_Bus.Nodes_ToId
			DropIndex("Bus.Requests", new[] { "FromId" });
            DropIndex("Bus.Requests", new[] { "ToId" });
            DropColumn("Bus.Requests", "FromId");
            DropColumn("Bus.Requests", "ToId");
        }
        
        public override void Down()
        {
            AddColumn("Bus.Requests", "ToId", c => c.Int(nullable: false));
            AddColumn("Bus.Requests", "FromId", c => c.Int(nullable: false));
            CreateIndex("Bus.Requests", "ToId");
            CreateIndex("Bus.Requests", "FromId");
            AddForeignKey("Bus.Requests", "ToId", "Bus.RequestStops", "Id");
            AddForeignKey("Bus.Requests", "FromId", "Bus.RequestStops", "Id");
        }
    }
}
