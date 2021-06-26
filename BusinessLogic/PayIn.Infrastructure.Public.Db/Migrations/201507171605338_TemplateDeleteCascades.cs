namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class TemplateDeleteCascades : DbMigration
	{
		public override void Up()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlTemplateChecks", "TemplateId", "dbo.ControlTemplates");
			//DropForeignKey("dbo.ControlTemplateItems", "TemplateId", "dbo.ControlTemplates");
			//AddForeignKey("dbo.ControlTemplateChecks", "TemplateId", "dbo.ControlTemplates", "Id", cascadeDelete: true);
			//AddForeignKey("dbo.ControlTemplateItems", "TemplateId", "dbo.ControlTemplates", "Id", cascadeDelete: true);
			//// XAVI MIGRATION
		}

		public override void Down()
		{
			//// XAVI MIGRATION
			//DropForeignKey("dbo.ControlTemplateItems", "TemplateId", "dbo.ControlTemplates");
			//DropForeignKey("dbo.ControlTemplateChecks", "TemplateId", "dbo.ControlTemplates");
			//AddForeignKey("dbo.ControlTemplateItems", "TemplateId", "dbo.ControlTemplates", "Id");
			//AddForeignKey("dbo.ControlTemplateChecks", "TemplateId", "dbo.ControlTemplates", "Id");
			//// XAVI MIGRATION
		}
	}
}
