namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongitudeLatitudePrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("dbo.Events", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("dbo.Notices", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("dbo.Notices", "Latitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notices", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Notices", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Events", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Events", "Longitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
