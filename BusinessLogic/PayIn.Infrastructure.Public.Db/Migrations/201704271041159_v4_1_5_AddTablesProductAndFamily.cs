namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_5_AddTablesProductAndFamily : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductFamilies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Photo = c.String(nullable: false),
                        PaymentConcessionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcessionId)
                .Index(t => t.PaymentConcessionId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.String(nullable: false),
                        Photo = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Stock = c.Long(nullable: false),
                        Price = c.Double(nullable: false),
                        Vat = c.Double(nullable: false),
                        state = c.Int(nullable: false),
                        FamilyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductFamilies", t => t.FamilyID)
                .Index(t => t.FamilyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductFamilies", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.Products", "FamilyID", "dbo.ProductFamilies");
            DropIndex("dbo.Products", new[] { "FamilyID" });
            DropIndex("dbo.ProductFamilies", new[] { "PaymentConcessionId" });
            DropTable("dbo.Products");
            DropTable("dbo.ProductFamilies");
        }
    }
}
