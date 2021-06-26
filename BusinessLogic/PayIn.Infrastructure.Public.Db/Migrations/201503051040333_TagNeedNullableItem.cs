namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagNeedNullableItem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ServiceTags", new[] { "ItemId" });
            AlterColumn("dbo.ServiceTags", "ItemId", c => c.Int());
            CreateIndex("dbo.ServiceTags", "ItemId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ServiceTags", new[] { "ItemId" });
            AlterColumn("dbo.ServiceTags", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceTags", "ItemId");
        }
    }
}
