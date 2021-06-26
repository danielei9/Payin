namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v2_0_0_PaymentOrderIdNowInt2 : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Payments", "Order", c => c.Int());
			DropColumn("dbo.Payments", "OrderId");
		}

		public override void Down()
		{
			AddColumn("dbo.Payments", "OrderId", c => c.String(nullable: false));
			DropColumn("dbo.Payments", "Order");
		}
	}
}
