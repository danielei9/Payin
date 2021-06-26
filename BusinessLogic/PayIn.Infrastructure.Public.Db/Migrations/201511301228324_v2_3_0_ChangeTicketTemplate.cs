namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_ChangeTicketTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketTemplates", "BackTextPosition", c => c.String(nullable: false));
            AddColumn("dbo.TicketTemplates", "DecimalCharDelimiter", c => c.Int(nullable: false));
            DropColumn("dbo.TicketTemplates", "LaterTextPosition");
            DropColumn("dbo.TicketTemplates", "DecimalChar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketTemplates", "DecimalChar", c => c.String(nullable: false));
            AddColumn("dbo.TicketTemplates", "LaterTextPosition", c => c.String(nullable: false));
            DropColumn("dbo.TicketTemplates", "DecimalCharDelimiter");
            DropColumn("dbo.TicketTemplates", "BackTextPosition");
        }
    }
}
