namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_AddPayinComissionInPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "PayinCommision", c => c.Decimal(nullable: false, defaultValue:1, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "PayinCommision");
        }
    }
}
