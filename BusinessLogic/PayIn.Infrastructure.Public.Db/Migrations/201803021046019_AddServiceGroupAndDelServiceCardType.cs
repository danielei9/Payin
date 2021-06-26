namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceGroupAndDelServiceCardType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AllProduct = c.Boolean(nullable: false),
                        AllEntranceType = c.Boolean(nullable: false),
                        CategoryId = c.Int(),
                        ServiceUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCategories", t => t.CategoryId)
                .ForeignKey("dbo.ServiceUsers", t => t.ServiceUser_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.ServiceUser_Id);
            
            CreateTable(
                "dbo.ServiceCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceGroupEntranceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntranceTypeId = c.Int(nullable: false),
                        GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceGroups", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ServiceGroupProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceGroups", t => t.GroupId)
                .Index(t => t.GroupId);
            
            DropColumn("dbo.ServiceCards", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceCards", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.ServiceGroupProducts", "GroupId", "dbo.ServiceGroups");
            DropForeignKey("dbo.ServiceGroupEntranceTypes", "GroupId", "dbo.ServiceGroups");
            DropForeignKey("dbo.ServiceGroups", "ServiceUser_Id", "dbo.ServiceUsers");
            DropForeignKey("dbo.ServiceGroups", "CategoryId", "dbo.ServiceCategories");
            DropIndex("dbo.ServiceGroupProducts", new[] { "GroupId" });
            DropIndex("dbo.ServiceGroupEntranceTypes", new[] { "GroupId" });
            DropIndex("dbo.ServiceGroups", new[] { "ServiceUser_Id" });
            DropIndex("dbo.ServiceGroups", new[] { "CategoryId" });
            DropTable("dbo.ServiceGroupProducts");
            DropTable("dbo.ServiceGroupEntranceTypes");
            DropTable("dbo.ServiceCategories");
            DropTable("dbo.ServiceGroups");
        }
    }
}
