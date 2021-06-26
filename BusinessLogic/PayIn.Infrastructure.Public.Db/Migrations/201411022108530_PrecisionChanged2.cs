namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecisionChanged2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServicePrices", "AmountMinute", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AlterColumn("dbo.ServicePrices", "AmountBase", c => c.Decimal(nullable: false, precision: 18, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServicePrices", "AmountBase", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ServicePrices", "AmountMinute", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
