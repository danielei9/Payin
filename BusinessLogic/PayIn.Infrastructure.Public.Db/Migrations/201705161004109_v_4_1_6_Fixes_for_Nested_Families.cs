namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_Fixes_for_Nested_Families : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductFamilies", "SuperFamilyId", c => c.Int());
            CreateIndex("dbo.ProductFamilies", "SuperFamilyId");
            AddForeignKey("dbo.ProductFamilies", "SuperFamilyId", "dbo.ProductFamilies", "Id");
            DropColumn("dbo.ProductFamilies", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductFamilies", "ParentId", c => c.Int());
            DropForeignKey("dbo.ProductFamilies", "SuperFamilyId", "dbo.ProductFamilies");
            DropIndex("dbo.ProductFamilies", new[] { "SuperFamilyId" });
            DropColumn("dbo.ProductFamilies", "SuperFamilyId");
        }
    }
}
