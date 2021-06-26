namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v3_3_0_GreyList : DbMigration
	{
		public override void Up()
		{
			RenameColumn("dbo.GreyLists", "SerialNumber", "Uid");
			RenameColumn("dbo.GreyLists", "AffectedCamp", "Field");
			RenameColumn("dbo.GreyLists", "EquipmentType", "Machine");
			DropColumn("dbo.GreyLists", "OperationNumber");
		}

		public override void Down()
		{
			AddColumn("dbo.GreyLists", "OperationNumber", c => c.Int(nullable: false));
			RenameColumn("dbo.GreyLists", "Machine", "EquipmentType");
			RenameColumn("dbo.GreyLists", "Field", "AffectedCamp");
			RenameColumn("dbo.GreyLists", "Uid", "SerialNumber");
		}
	}
}
