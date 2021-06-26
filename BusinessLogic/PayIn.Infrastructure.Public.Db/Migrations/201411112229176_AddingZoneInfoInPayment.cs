namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddingZoneInfoInPayment : DbMigration
	{
		public override void Up()
    {
      AddColumn("dbo.Tickets", "ZoneId", c => c.Int());
      CreateIndex("dbo.Tickets", "ZoneId");
      AddForeignKey("dbo.Tickets", "ZoneId", "dbo.ServiceZones", "Id");
			Sql(
				"update dbo.Tickets " +
				"set ZoneId = 1 "
			); 
			Sql(
				"update dbo.Tickets " +
				"set ZoneName = Z.Name " +
				"from " +
					"dbo.Tickets T inner join " +
					"dbo.ServiceZones Z on T.ZoneId = Z.Id "
			);
    }

		public override void Down()
		{
			DropForeignKey("dbo.Tickets", "ZoneId", "dbo.ServiceZones");
			DropIndex("dbo.Tickets", new[] { "ZoneId" });
			DropColumn("dbo.Tickets", "ZoneId");
		}
	}
}
