namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductFamilyNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "FamilyId" });
            AlterColumn("dbo.Products", "FamilyId", c => c.Int());
            CreateIndex("dbo.Products", "FamilyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "FamilyId" });
            AlterColumn("dbo.Products", "FamilyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "FamilyId");
        }
    }
}
