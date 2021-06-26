namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Type", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "Type");
        }
    }
}
