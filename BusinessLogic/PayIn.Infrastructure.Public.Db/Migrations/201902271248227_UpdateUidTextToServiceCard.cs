namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUidTextToServiceCard : DbMigration
    {
        public override void Up()
        {
			Sql(
				"UPDATE ServiceCards " +
			);
        }
        
        public override void Down()
        {
        }
    }
}