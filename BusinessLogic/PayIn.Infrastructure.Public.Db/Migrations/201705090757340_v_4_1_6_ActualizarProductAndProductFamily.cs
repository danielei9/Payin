namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_ActualizarProductAndProductFamily : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductFamilies", "ParentId", c => c.Int());
            AddColumn("dbo.ProductFamilies", "state", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "PaymentConcessionId", c => c.Int());
            CreateIndex("dbo.Products", "PaymentConcessionId");
            AddForeignKey("dbo.Products", "PaymentConcessionId", "dbo.PaymentConcessions", "Id");
            RenameColumn("dbo.Products", "Photo", "PhotoUrl");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.Products", new[] { "PaymentConcessionId" });
            DropColumn("dbo.Products", "PaymentConcessionId");
            RenameColumn("dbo.Products", "PhotoUrl", "Photo");
            DropColumn("dbo.ProductFamilies", "state");
            DropColumn("dbo.ProductFamilies", "ParentId");
        }
    }
}
