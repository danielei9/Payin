namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolucionandoError2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.Requests", "FromId", c => c.Int(nullable: false));
            AddColumn("Bus.Requests", "ToId", c => c.Int(nullable: false));
            CreateIndex("Bus.Requests", "FromId");
            CreateIndex("Bus.Requests", "ToId");
            AddForeignKey("Bus.Requests", "FromId", "Bus.RequestStops", "Id");
            AddForeignKey("Bus.Requests", "ToId", "Bus.RequestStops", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.Requests", "ToId", "Bus.RequestStops");
            DropForeignKey("Bus.Requests", "FromId", "Bus.RequestStops");
            DropIndex("Bus.Requests", new[] { "ToId" });
            DropIndex("Bus.Requests", new[] { "FromId" });
            DropColumn("Bus.Requests", "ToId");
            DropColumn("Bus.Requests", "FromId");
        }
    }
}
