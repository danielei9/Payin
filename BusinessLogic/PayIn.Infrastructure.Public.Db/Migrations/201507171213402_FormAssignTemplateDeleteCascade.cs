namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class FormAssignTemplateDeleteCascade : DbMigration
	{
		public override void Up()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlFormAssignTemplates", "CheckId", "dbo.ControlTemplateChecks");
			//AddForeignKey("dbo.ControlFormAssignTemplates", "CheckId", "dbo.ControlTemplateChecks", "Id", cascadeDelete: true);
			//// XAVI MIGRATION
		}

		public override void Down()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlFormAssignTemplates", "CheckId", "dbo.ControlTemplateChecks");
			//AddForeignKey("dbo.ControlFormAssignTemplates", "CheckId", "dbo.ControlTemplateChecks", "Id");
			//// XAVI MIGRATION
		}
	}
}
