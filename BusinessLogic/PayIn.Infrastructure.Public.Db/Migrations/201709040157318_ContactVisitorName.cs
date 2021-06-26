namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactVisitorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "VisitorName", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "VisitorName");
        }
    }
}
