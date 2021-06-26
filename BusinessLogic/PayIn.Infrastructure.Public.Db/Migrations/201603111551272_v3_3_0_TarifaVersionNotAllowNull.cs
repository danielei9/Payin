namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v3_3_0_TarifaVersionNotAllowNull : DbMigration
	{
		public override void Up()
		{
			Sql("DELETE dbo.TransportPrices WHERE Version IS NULL");
			AlterColumn("dbo.TransportPrices", "Version", c => c.Int(nullable: false));
		}

		public override void Down()
		{
			AlterColumn("dbo.TransportPrices", "Version", c => c.Int());
		}
	}
}
