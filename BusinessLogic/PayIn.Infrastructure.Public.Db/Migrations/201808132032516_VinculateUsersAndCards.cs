namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VinculateUsersAndCards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCardServiceUsers",
                c => new
                    {
                        ServiceCard_Id = c.Int(nullable: false),
                        ServiceUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceCard_Id, t.ServiceUser_Id })
                .ForeignKey("dbo.ServiceCards", t => t.ServiceCard_Id, cascadeDelete: true)
                .ForeignKey("dbo.ServiceUsers", t => t.ServiceUser_Id, cascadeDelete: true)
                .Index(t => t.ServiceCard_Id)
                .Index(t => t.ServiceUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCardServiceUsers", "ServiceUser_Id", "dbo.ServiceUsers");
            DropForeignKey("dbo.ServiceCardServiceUsers", "ServiceCard_Id", "dbo.ServiceCards");
            DropIndex("dbo.ServiceCardServiceUsers", new[] { "ServiceUser_Id" });
            DropIndex("dbo.ServiceCardServiceUsers", new[] { "ServiceCard_Id" });
            DropTable("dbo.ServiceCardServiceUsers");
        }
    }
}
