namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corrigiendocollection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceGroups", "ServiceUser_Id", "dbo.ServiceUsers");
            DropIndex("dbo.ServiceGroups", new[] { "ServiceUser_Id" });
            CreateTable(
                "dbo.ServiceGroupServiceUsers",
                c => new
                    {
                        ServiceGroup_Id = c.Int(nullable: false),
                        ServiceUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceGroup_Id, t.ServiceUser_Id })
                .ForeignKey("dbo.ServiceGroups", t => t.ServiceGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.ServiceUsers", t => t.ServiceUser_Id, cascadeDelete: true)
                .Index(t => t.ServiceGroup_Id)
                .Index(t => t.ServiceUser_Id);
            
            DropColumn("dbo.ServiceGroups", "ServiceUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceGroups", "ServiceUser_Id", c => c.Int());
            DropForeignKey("dbo.ServiceGroupServiceUsers", "ServiceUser_Id", "dbo.ServiceUsers");
            DropForeignKey("dbo.ServiceGroupServiceUsers", "ServiceGroup_Id", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceGroupServiceUsers", new[] { "ServiceUser_Id" });
            DropIndex("dbo.ServiceGroupServiceUsers", new[] { "ServiceGroup_Id" });
            DropTable("dbo.ServiceGroupServiceUsers");
            CreateIndex("dbo.ServiceGroups", "ServiceUser_Id");
            AddForeignKey("dbo.ServiceGroups", "ServiceUser_Id", "dbo.ServiceUsers", "Id");
        }
    }
}
