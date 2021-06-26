namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SexTypeNotString : DbMigration
    {
        public override void Up()
        {
			AddColumn("security.AspNetUsers", "SexAux", c => c.Int());

			Sql(
				"UPDATE ANU " +
				"SET SexAux = 1 " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex = 'Male' "
			);
			Sql(
				"UPDATE ANU " +
				"SET SexAux = 2 " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex = 'Female' "
			);
			Sql(
				"UPDATE ANU " +
				"SET SexAux = 0 " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex <> 'Male' AND ANU.Sex <> 'Female' "
			);

            DropColumn("security.AspNetUsers", "Sex");
            AddColumn("security.AspNetUsers", "Sex", c => c.Int(nullable: false));

			Sql(
				"UPDATE ANU " +
				"SET Sex = SexAux " +
				"FROM security.AspNetUsers ANU "
			);

			DropColumn("security.AspNetUsers", "SexAux");
        }
        
        public override void Down()
        {
			AddColumn("security.AspNetUsers", "SexAux", c => c.String());

			Sql(
				"UPDATE ANU " +
				"SET SexAux = 'Male' " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex = 1 "
			);
			Sql(
				"UPDATE ANU " +
				"SET SexAux = 'Female' " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex = 2 "
			);
			Sql(
				"UPDATE ANU " +
				"SET SexAux = '' " +
				"FROM security.AspNetUsers ANU " +
				"WHERE ANU.Sex <> 1 AND ANU.Sex <> 2 "
			);

            DropColumn("security.AspNetUsers", "Sex");
            AddColumn("security.AspNetUsers", "Sex", c => c.String(nullable: false));

			Sql(
				"UPDATE ANU " +
				"SET Sex = SexAux " +
				"FROM security.AspNetUsers ANU "
			);

			DropColumn("security.AspNetUsers", "SexAux");
        }
    }
}
