namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPresencePositionNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlPresences", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("dbo.ControlPresences", "Longitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlPresences", "Longitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
            AlterColumn("dbo.ControlPresences", "Latitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
        }
    }
}
