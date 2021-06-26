namespace PayIn.Infrastructure.Security.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class UserInformation2 : DbMigration
	{
		public override void Up()
		{
			AddColumn("security.AspNetUsers", "Name", c => c.String(nullable: false));
			Sql("UPDATE security.AspNetUsers SET Name=FirstName + ' ' + LastName;");
			Sql("UPDATE security.AspNetUsers SET Email='' WHERE Email IS NULL;");
			DropColumn("security.AspNetUsers", "FirstName");
			DropColumn("security.AspNetUsers", "LastName");
		}

		public override void Down()
		{
			AddColumn("security.AspNetUsers", "LastName", c => c.String(nullable: false));
			AddColumn("security.AspNetUsers", "FirstName", c => c.String(nullable: false));
			Sql("UPDATE security.AspNetUsers SET FirstName=Name;");
			DropColumn("security.AspNetUsers", "Name");
		}
	}
}
