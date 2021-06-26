namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleQuantityDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportTitles", "Quantity", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportTitles", "Quantity", c => c.Int());
        }
    }
}
