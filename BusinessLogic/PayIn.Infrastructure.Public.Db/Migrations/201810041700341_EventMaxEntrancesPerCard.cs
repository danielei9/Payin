namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventMaxEntrancesPerCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "MaxAmountToSpend", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "MaxAmountToSpend");
        }
    }
}
