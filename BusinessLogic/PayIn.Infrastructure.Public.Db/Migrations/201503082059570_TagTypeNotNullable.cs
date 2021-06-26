namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class TagTypeNotNullable : DbMigration
	{
		public override void Up()
		{
			Sql("UPDATE dbo.ServiceTags SET TagType=1 WHERE TagType IS NULL");
			AlterColumn("dbo.ServiceTags", "TagType", c => c.Int(nullable: false, defaultValue: 1));
		}

		public override void Down()
		{
			AlterColumn("dbo.ServiceTags", "TagType", c => c.Int());
		}
	}
}
