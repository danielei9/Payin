namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesAndTimestampToRequest : DbMigration
    {
        public override void Up()
		{
			AddColumn("Bus.Requests", "Note", c => c.String(nullable: true));
			AddColumn("Bus.Requests", "Timestamp", c => c.DateTime(nullable: true));

			Sql(
				"UPDATE Bus.Requests " +
				"SET Note='', Timestamp=GETDATE()"
			);

			AlterColumn("Bus.Requests", "Note", c => c.String(nullable: false));
            AlterColumn("Bus.Requests", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Bus.Requests", "Timestamp");
            DropColumn("Bus.Requests", "Note");
        }
    }
}
