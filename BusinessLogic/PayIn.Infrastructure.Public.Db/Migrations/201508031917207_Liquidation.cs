namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Liquidation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Liquidations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.Int(nullable: false),
                        LiquidationDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Payments", "LiquidationId", c => c.Int());
            CreateIndex("dbo.Payments", "LiquidationId");
            AddForeignKey("dbo.Payments", "LiquidationId", "dbo.Liquidations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "LiquidationId", "dbo.Liquidations");
            DropIndex("dbo.Payments", new[] { "LiquidationId" });
            DropColumn("dbo.Payments", "LiquidationId");
            DropTable("dbo.Liquidations");
        }
    }
}
