namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v2_0_0_ChangePaymentEnumValues : DbMigration
	{
		public override void Up()
		{
			Sql("UPDATE Payments SET State=1 WHERE State=0");
		}

		public override void Down()
		{
			Sql("UPDATE Payments SET State=0 WHERE State=1");
		}
	}
}
