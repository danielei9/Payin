namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketWithExternalLogin1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "ExternalLogin", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "ExternalLogin");
        }
    }
}
