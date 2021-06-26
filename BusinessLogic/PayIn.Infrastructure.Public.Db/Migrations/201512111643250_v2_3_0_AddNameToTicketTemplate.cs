namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_AddNameToTicketTemplate : DbMigration
    {
        public override void Up()
		{
			AddColumn("dbo.TicketTemplates", "Name", c => c.String(nullable: true));
			Sql(
				"UPDATE dbo.TicketTemplates " +
				"SET Name = 'Default' " 
			);
			AlterColumn("dbo.TicketTemplates", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketTemplates", "Name");
        }
    }
}
