namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_AddDecimalCharToTicketTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketTemplates", "DecimalChar", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketTemplates", "DecimalChar");
        }
    }
}
