namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ItemTagMultivaluated : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.ControlItemServiceTags",
				c => new
				{
					ControlItem_Id = c.Int(nullable: false),
					ServiceTag_Id = c.Int(nullable: false),
				})
				.PrimaryKey(t => new { t.ControlItem_Id, t.ServiceTag_Id })
				.ForeignKey("dbo.ControlItems", t => t.ControlItem_Id, cascadeDelete: true)
				.ForeignKey("dbo.ServiceTags", t => t.ServiceTag_Id, cascadeDelete: true)
				.Index(t => t.ControlItem_Id)
				.Index(t => t.ServiceTag_Id);
			Sql(
				"INSERT dbo.ControlItemServiceTags " + 
					"(ControlItem_Id, ServiceTag_Id) " +
				"SELECT ST.ItemId, ST.Id " +
				"FROM dbo.ServiceTags ST " + 
				"WHERE ST.ItemId IS NOT NULL"
			);

			DropForeignKey("dbo.ServiceTags", "ItemId", "dbo.ControlItems");
			DropIndex("dbo.ServiceTags", new[] { "ItemId" });


			DropColumn("dbo.ServiceTags", "ItemId");
		}

		public override void Down()
		{
			AddColumn("dbo.ServiceTags", "ItemId", c => c.Int());
			DropForeignKey("dbo.ControlItemServiceTags", "ServiceTag_Id", "dbo.ServiceTags");
			DropForeignKey("dbo.ControlItemServiceTags", "ControlItem_Id", "dbo.ControlItems");
			DropIndex("dbo.ControlItemServiceTags", new[] { "ServiceTag_Id" });
			DropIndex("dbo.ControlItemServiceTags", new[] { "ControlItem_Id" });
			DropTable("dbo.ControlItemServiceTags");
			CreateIndex("dbo.ServiceTags", "ItemId");
			AddForeignKey("dbo.ServiceTags", "ItemId", "dbo.ControlItems", "Id");
		}
	}
}
