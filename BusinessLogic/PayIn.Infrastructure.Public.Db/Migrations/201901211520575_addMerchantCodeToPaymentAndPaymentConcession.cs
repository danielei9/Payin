namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMerchantCodeToPaymentAndPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "MerchantCode", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.Payments", "MerchantCode", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "MerchantCode");
            DropColumn("dbo.PaymentConcessions", "MerchantCode");
        }
    }
}
