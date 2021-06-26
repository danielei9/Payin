namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidationPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Liquidations", "Since", c => c.DateTime(nullable: false));
            AddColumn("dbo.Liquidations", "Until", c => c.DateTime(nullable: false));
            AddColumn("dbo.Liquidations", "RequestDate", c => c.DateTime());
            AddColumn("dbo.PaymentConcessions", "FormUrl", c => c.String(nullable: false));
            AddColumn("dbo.PaymentConcessions", "LiquidationRequestDate", c => c.DateTime());
            AddColumn("dbo.PaymentConcessions", "LiquidationAmountMin", c => c.Decimal(nullable: false, precision: 9, scale: 6));

			Sql("UPDATE PaymentConcessions SET LiquidationAmountMin=100.0 ");

			AlterColumn("dbo.Liquidations", "PaymentDate", c => c.DateTime());
            DropColumn("dbo.Liquidations", "LiquidationDate");
            DropColumn("dbo.PaymentConcessions", "FormA");
		}
        
        public override void Down()
        {
            AddColumn("dbo.PaymentConcessions", "FormA", c => c.Binary(nullable: false));
            AddColumn("dbo.Liquidations", "LiquidationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Liquidations", "PaymentDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.PaymentConcessions", "LiquidationAmountMin");
            DropColumn("dbo.PaymentConcessions", "LiquidationRequestDate");
            DropColumn("dbo.PaymentConcessions", "FormUrl");
            DropColumn("dbo.Liquidations", "RequestDate");
            DropColumn("dbo.Liquidations", "Until");
            DropColumn("dbo.Liquidations", "Since");
        }
    }
}
