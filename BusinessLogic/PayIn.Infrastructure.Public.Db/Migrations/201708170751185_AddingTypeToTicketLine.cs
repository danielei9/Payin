namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTypeToTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "Type", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketLines", "Type");
        }
    }
}
