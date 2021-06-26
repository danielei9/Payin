namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPlanningCheckDurationType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlPlannings", "CheckDuration", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlPlannings", "CheckDuration", c => c.Time(precision: 7));
        }
    }
}
