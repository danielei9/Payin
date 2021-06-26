namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v3_3_1_BlackListUidLong : DbMigration
	{
		public override void Up()
		{
			AlterColumn("dbo.BlackLists", "Uid", c => c.Long(nullable: false));
		}

		public override void Down()
		{
			AlterColumn("dbo.BlackLists", "Uid", c => c.Long(nullable: false));
		}
	}
}
