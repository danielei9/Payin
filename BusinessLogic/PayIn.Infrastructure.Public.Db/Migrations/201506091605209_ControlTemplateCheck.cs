namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlTemplateCheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlTemplateChecks", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlTemplateChecks", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlTemplateChecks", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlTemplateChecks", "Time");
        }
    }
}
