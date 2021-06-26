namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add6DecimalsToCampaignLineQuantity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CampaignLines", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CampaignLines", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
