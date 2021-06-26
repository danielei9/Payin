namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddZoneAndConcessionInfoToTicket : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Tickets", "SupplierName", c => c.String(nullable: false));
			AddColumn("dbo.Tickets", "ConcessionName", c => c.String(nullable: false));
			AddColumn("dbo.Tickets", "ZoneName", c => c.String(nullable: false));

			RenameColumn("dbo.Tickets", "TaxName", "SupplierTaxName");
			RenameColumn("dbo.Tickets", "TaxAddress", "SupplierTaxAddress");
			RenameColumn("dbo.Tickets", "TaxNumber", "SupplierTaxNumber");
		}

		public override void Down()
		{
			DropColumn("dbo.Tickets", "ZoneName");
			DropColumn("dbo.Tickets", "ConcessionName");
			DropColumn("dbo.Tickets", "SupplierName");

			RenameColumn("dbo.Tickets", "SupplierTaxName", "TaxName");
			RenameColumn("dbo.Tickets", "SupplierTaxAddress", "TaxAddress");
			RenameColumn("dbo.Tickets", "SupplierTaxNumber", "TaxNumber");

		}
	}
}
