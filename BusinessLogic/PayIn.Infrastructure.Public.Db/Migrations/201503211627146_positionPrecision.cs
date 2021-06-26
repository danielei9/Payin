namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class positionPrecision : DbMigration
	{
		public override void Up()
		{
			RenameColumn("dbo.ServiceTags", "Observation", "Observations");
			AlterColumn("dbo.ServiceTags", "Latitude", c => c.Decimal(precision: 9, scale: 6));
			AlterColumn("dbo.ServiceTags", "Longitude", c => c.Decimal(precision: 9, scale: 6));
			AlterColumn("dbo.ControlPresences", "Latitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
			AlterColumn("dbo.ControlPresences", "Longitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
			AlterColumn("dbo.ControlPresences", "LatitudeWanted", c => c.Decimal(precision: 9, scale: 6));
			AlterColumn("dbo.ControlPresences", "LongitudeWanted", c => c.Decimal(precision: 9, scale: 6));
			AlterColumn("dbo.ServicePrices", "Price", c => c.Decimal(nullable: false, precision: 9, scale: 3));
		}

		public override void Down()
		{
			RenameColumn("dbo.ServiceTags", "Observations", "Observation");
			AlterColumn("dbo.ServicePrices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 8));
			AlterColumn("dbo.ControlPresences", "LongitudeWanted", c => c.Decimal(precision: 18, scale: 2));
			AlterColumn("dbo.ControlPresences", "LatitudeWanted", c => c.Decimal(precision: 18, scale: 2));
			AlterColumn("dbo.ControlPresences", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
			AlterColumn("dbo.ControlPresences", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
			AlterColumn("dbo.ServiceTags", "Longitude", c => c.Decimal(precision: 18, scale: 2));
			AlterColumn("dbo.ServiceTags", "Latitude", c => c.Decimal(precision: 18, scale: 2));
		}
	}
}
