namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_ChangeTransportTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportConcessions", "TransportSystemId", c => c.Int());
            AddColumn("dbo.TransportTitles", "TransportConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.TransportTitles", "TransportConcessionId");
            CreateIndex("dbo.TransportConcessions", "TransportSystemId");
            AddForeignKey("dbo.TransportTitles", "TransportConcessionId", "dbo.TransportConcessions", "Id");
            AddForeignKey("dbo.TransportConcessions", "TransportSystemId", "dbo.TransportSystems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportConcessions", "TransportSystemId", "dbo.TransportSystems");
            DropForeignKey("dbo.TransportTitles", "TransportConcessionId", "dbo.TransportConcessions");
            DropIndex("dbo.TransportConcessions", new[] { "TransportSystemId" });
            DropIndex("dbo.TransportTitles", new[] { "TransportConcessionId" });
            DropColumn("dbo.TransportTitles", "TransportConcessionId");
            DropColumn("dbo.TransportConcessions", "TransportSystemId");
        }
    }
}
