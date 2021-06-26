namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateItemSinceUntilType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlTemplateItems", "Since", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ControlTemplateItems", "Until", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlTemplateItems", "Until", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.ControlTemplateItems", "Since", c => c.Time(nullable: false, precision: 7));
        }
    }
}
