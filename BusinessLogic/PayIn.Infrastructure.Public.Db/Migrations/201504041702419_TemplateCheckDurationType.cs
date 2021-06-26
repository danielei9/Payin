namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateCheckDurationType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlTemplates", "CheckDuration", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlTemplates", "CheckDuration", c => c.Time(precision: 7));
        }
    }
}
