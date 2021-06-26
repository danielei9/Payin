namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCardVinculations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceCardServiceUsers", "ServiceCard_Id", "dbo.ServiceCards");
            DropForeignKey("dbo.ServiceCardServiceUsers", "ServiceUser_Id", "dbo.ServiceUsers");
            DropIndex("dbo.ServiceCardServiceUsers", new[] { "ServiceCard_Id" });
            DropIndex("dbo.ServiceCardServiceUsers", new[] { "ServiceUser_Id" });
            CreateTable(
                "dbo.ServiceUserVinculations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        CardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCards", t => t.CardId)
                .Index(t => t.CardId);
            
            DropTable("dbo.ServiceCardServiceUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceCardServiceUsers",
                c => new
                    {
                        ServiceCard_Id = c.Int(nullable: false),
                        ServiceUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceCard_Id, t.ServiceUser_Id });
            
            DropForeignKey("dbo.ServiceUserVinculations", "CardId", "dbo.ServiceCards");
            DropIndex("dbo.ServiceUserVinculations", new[] { "CardId" });
            DropTable("dbo.ServiceUserVinculations");
            CreateIndex("dbo.ServiceCardServiceUsers", "ServiceUser_Id");
            CreateIndex("dbo.ServiceCardServiceUsers", "ServiceCard_Id");
            AddForeignKey("dbo.ServiceCardServiceUsers", "ServiceUser_Id", "dbo.ServiceUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceCardServiceUsers", "ServiceCard_Id", "dbo.ServiceCards", "Id", cascadeDelete: true);
        }
    }
}
