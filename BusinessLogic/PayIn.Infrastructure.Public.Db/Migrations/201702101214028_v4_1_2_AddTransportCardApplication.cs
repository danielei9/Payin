namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_2_AddTransportCardApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportCardApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(),
                        KeyVersion = c.Int(nullable: false),
                        Content = c.String(),
                        AccessCondition = c.String(),
                        ReadKey = c.Int(nullable: false),
                        WriteKey = c.Int(nullable: false),
                        TransportSystemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportSystems", t => t.TransportSystemId)
                .Index(t => t.TransportSystemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportCardApplications", "TransportSystemId", "dbo.TransportSystems");
            DropIndex("dbo.TransportCardApplications", new[] { "TransportSystemId" });
            DropTable("dbo.TransportCardApplications");
        }
    }
}
