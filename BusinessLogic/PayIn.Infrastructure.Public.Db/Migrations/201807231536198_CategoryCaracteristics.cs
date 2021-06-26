namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryCaracteristics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCategories", "AllMembersInSomeGroup", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceCategories", "AMemberInOnlyOneGroup", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceCategories", "AskWhenEmit", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceCategories", "DefaultGroupWhenEmitId", c => c.Int());
            CreateIndex("dbo.ServiceCategories", "DefaultGroupWhenEmitId");
            AddForeignKey("dbo.ServiceCategories", "DefaultGroupWhenEmitId", "dbo.ServiceGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCategories", "DefaultGroupWhenEmitId", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceCategories", new[] { "DefaultGroupWhenEmitId" });
            DropColumn("dbo.ServiceCategories", "DefaultGroupWhenEmitId");
            DropColumn("dbo.ServiceCategories", "AskWhenEmit");
            DropColumn("dbo.ServiceCategories", "AMemberInOnlyOneGroup");
            DropColumn("dbo.ServiceCategories", "AllMembersInSomeGroup");
        }
    }
}
