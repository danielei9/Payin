namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_AnyadirLineasATickets : DbMigration
    {
        public override void Up()
        {
			Sql(
				"INSERT INTO [dbo].[TicketLines] ([Title],[Reference],[Amount],[Quantity],[TicketId]) " +
				"SELECT T.Title, T.Reference, T.Amount, 1, T.id " +
				"FROM[dbo].[Tickets] T"
			);
            DropColumn("dbo.Tickets", "Title");
            DropColumn("dbo.TicketLines", "Reference");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketLines", "Reference", c => c.String());
            AddColumn("dbo.Tickets", "Title", c => c.String(nullable: false));
        }
    }
}
