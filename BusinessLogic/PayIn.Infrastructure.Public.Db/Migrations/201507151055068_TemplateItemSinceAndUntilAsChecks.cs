namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class TemplateItemSinceAndUntilAsChecks : DbMigration
	{
		public override void Up()
		{
			// XAVI MIGRATION
			DropIndex("dbo.ControlTemplateChecks", new[] { "CheckPointId" });
			AddColumn("dbo.ControlTemplateItems", "SinceId", c => c.Int(nullable: false));
			AddColumn("dbo.ControlTemplateItems", "UntilId", c => c.Int(nullable: false));
			AlterColumn("dbo.ControlTemplateChecks", "CheckPointId", c => c.Int());
			CreateIndex("dbo.ControlTemplateChecks", "CheckPointId");

			// A MANO
			AddColumn("dbo.ControlTemplateChecks", "Temp", c => c.Int(nullable: true));
			Sql(
				"INSERT dbo.ControlTemplateChecks (Time, TemplateId, Temp, CheckPointId) " +
				"SELECT CTI.since, CTI.templateId, CTI.id, null FROM dbo.ControlTemplateItems CTI"
			);
			Sql(
				"UPDATE CTI " +
				"SET SinceId = CTC.Id " +
				"FROM " +
					"dbo.ControlTemplateItems CTI INNER JOIN " +
					"dbo.ControlTemplateChecks CTC ON CTC.Temp = CTI.id"
			);
			Sql("UPDATE dbo.ControlTemplateChecks SET Temp = NULL");
			Sql(
				"INSERT dbo.ControlTemplateChecks (Time, TemplateId, Temp, CheckPointId) " +
				"SELECT CTI.until, CTI.templateId, CTI.id, null FROM dbo.ControlTemplateItems CTI"
			);
			Sql(
				"UPDATE CTI " +
				"SET UntilId = CTC.Id " +
				"FROM " +
					"dbo.ControlTemplateItems CTI INNER JOIN " +
					"dbo.ControlTemplateChecks CTC ON CTC.Temp = CTI.id"
			);
			DropColumn("dbo.ControlTemplateChecks", "Temp");
			// FIN A MANO

			CreateIndex("dbo.ControlTemplateItems", "SinceId");
			CreateIndex("dbo.ControlTemplateItems", "UntilId");
			AddForeignKey("dbo.ControlTemplateItems", "SinceId", "dbo.ControlTemplateChecks", "Id");
			AddForeignKey("dbo.ControlTemplateItems", "UntilId", "dbo.ControlTemplateChecks", "Id");
			DropColumn("dbo.ControlTemplateItems", "Since");
			DropColumn("dbo.ControlTemplateItems", "Until");
			// XAVI MIGRATION
		}

		public override void Down()
		{
			// XAVI MIGRATION
			AddColumn("dbo.ControlTemplateItems", "Until", c => c.DateTime(nullable: false));
			AddColumn("dbo.ControlTemplateItems", "Since", c => c.DateTime(nullable: false));

			Sql(
				"UPDATE CTI " +
				"SET Since = S.time, Until = U.time " +
				"FROM " +
					"dbo.ControlTemplateItems CTI LEFT JOIN " +
					"dbo.ControlTemplateChecks S ON CTI.SinceId = S.id LEFT JOIN " +
					"dbo.ControlTemplateChecks U ON CTI.UntilId = U.id "
			);
			Sql(
				"DELETE FROM dbo.ControlTemplateChecks " +
				"WHERE EXISTS " +
					"(SELECT * " +
					"FROM dbo.ControlTemplateItems CTI " +
					"WHERE CTI.SinceId = dbo.ControlTemplateChecks.id OR CTI.UntilId = dbo.ControlTemplateChecks.id " +
					")"
			);

			DropForeignKey("dbo.ControlTemplateItems", "UntilId", "dbo.ControlTemplateChecks");
			DropForeignKey("dbo.ControlTemplateItems", "SinceId", "dbo.ControlTemplateChecks");
			DropIndex("dbo.ControlTemplateItems", new[] { "UntilId" });
			DropIndex("dbo.ControlTemplateItems", new[] { "SinceId" });
			DropIndex("dbo.ControlTemplateChecks", new[] { "CheckPointId" });
			AlterColumn("dbo.ControlTemplateChecks", "CheckPointId", c => c.Int(nullable: false));
			DropColumn("dbo.ControlTemplateItems", "UntilId");
			DropColumn("dbo.ControlTemplateItems", "SinceId");
			CreateIndex("dbo.ControlTemplateChecks", "CheckPointId");
			// XAVI MIGRATION
		}
	}
}
