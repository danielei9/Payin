namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewControlPresence : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlPresences", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ControlPresences", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlPresences", "Longitude", c => c.Long(nullable: false));
            AlterColumn("dbo.ControlPresences", "Latitude", c => c.Long(nullable: false));
        }
    }
}
