namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlItemsCheckTimetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlItems", "CheckTimetable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlItems", "CheckTimetable");
        }
    }
}
