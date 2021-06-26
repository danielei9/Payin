namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class PriceTableChange : DbMigration
	{
		public override void Up()
		{
			Sql("DELETE dbo.ServicePrices");
			AddColumn("dbo.ServicePrices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 8));
			AddColumn("dbo.ServicePrices", "Time", c => c.Time(nullable: false, precision: 7));
			DropColumn("dbo.ServicePrices", "AmountMinute");
			DropColumn("dbo.ServicePrices", "AmountBase");
			DropColumn("dbo.ServicePrices", "FromTime");
			DropColumn("dbo.ServicePrices", "UntilTime");
		}

		public override void Down()
		{
			Sql("DELETE dbo.ServicePrices");
			AddColumn("dbo.ServicePrices", "UntilTime", c => c.Time(nullable: false, precision: 7));
			AddColumn("dbo.ServicePrices", "FromTime", c => c.Time(nullable: false, precision: 7));
			AddColumn("dbo.ServicePrices", "AmountBase", c => c.Decimal(nullable: false, precision: 18, scale: 8));
			AddColumn("dbo.ServicePrices", "AmountMinute", c => c.Decimal(nullable: false, precision: 18, scale: 8));
			DropColumn("dbo.ServicePrices", "Time");
			DropColumn("dbo.ServicePrices", "Price");
		}
	}
}
