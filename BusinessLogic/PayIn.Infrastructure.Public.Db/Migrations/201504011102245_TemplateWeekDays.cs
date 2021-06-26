namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateWeekDays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlTemplates", "Monday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Tuesday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Wednesday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Thursday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Friday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Saturday", c => c.Boolean(nullable: false, defaultValue: true));
						AddColumn("dbo.ControlTemplates", "Sunday", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlTemplates", "Sunday");
            DropColumn("dbo.ControlTemplates", "Saturday");
            DropColumn("dbo.ControlTemplates", "Friday");
            DropColumn("dbo.ControlTemplates", "Thursday");
            DropColumn("dbo.ControlTemplates", "Wednesday");
            DropColumn("dbo.ControlTemplates", "Tuesday");
            DropColumn("dbo.ControlTemplates", "Monday");
        }
    }
}
