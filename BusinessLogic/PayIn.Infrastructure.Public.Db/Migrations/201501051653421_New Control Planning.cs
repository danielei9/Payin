namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewControlPlanning : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlPlannings", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ControlPlannings", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlPlannings", "Longitude", c => c.Long(nullable: false));
            AlterColumn("dbo.ControlPlannings", "Latitude", c => c.Long(nullable: false));
        }
    }
}
