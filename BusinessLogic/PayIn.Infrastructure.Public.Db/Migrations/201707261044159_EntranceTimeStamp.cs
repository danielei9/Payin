namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceTimeStamp : DbMigration
    {
        public override void Up()
		{
			AddColumn("dbo.Entrances", "Timestamp", c => c.DateTime(nullable: true));
			Sql(
				"UPDATE dbo.Entrances " +
				"SET Timestamp = T.Date " +
				"FROM " + 
					"dbo.Entrances E INNER JOIN " +
					"dbo.TicketLines TL ON TL.Id = E.TicketLineId INNER JOIN " +
					"dbo.Tickets T ON TL.ticketId = T.Id "
			);
			AlterColumn("dbo.Entrances", "Timestamp", c => c.DateTime(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.Entrances", "Timestamp");
        }
    }
}
