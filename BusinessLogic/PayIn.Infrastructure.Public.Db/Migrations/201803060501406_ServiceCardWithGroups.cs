namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCardWithGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceGroupServiceCards",
                c => new
                    {
                        ServiceGroup_Id = c.Int(nullable: false),
                        ServiceCard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceGroup_Id, t.ServiceCard_Id })
                .ForeignKey("dbo.ServiceGroups", t => t.ServiceGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCards", t => t.ServiceCard_Id, cascadeDelete: true)
                .Index(t => t.ServiceGroup_Id)
                .Index(t => t.ServiceCard_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceGroupServiceCards", "ServiceCard_Id", "dbo.ServiceCards");
            DropForeignKey("dbo.ServiceGroupServiceCards", "ServiceGroup_Id", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceGroupServiceCards", new[] { "ServiceCard_Id" });
            DropIndex("dbo.ServiceGroupServiceCards", new[] { "ServiceGroup_Id" });
            DropTable("dbo.ServiceGroupServiceCards");
        }
    }
}
