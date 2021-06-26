namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidationConcessionToPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "LiquidationConcessionId", c => c.Int());
            CreateIndex("dbo.PaymentConcessions", "LiquidationConcessionId");
            AddForeignKey("dbo.PaymentConcessions", "LiquidationConcessionId", "dbo.PaymentConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentConcessions", "LiquidationConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.PaymentConcessions", new[] { "LiquidationConcessionId" });
            DropColumn("dbo.PaymentConcessions", "LiquidationConcessionId");
        }
    }
}
