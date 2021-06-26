namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PinHash : DbMigration
    {
        public override void Up()
        {
					var pin = "1234".ToHash();
					Sql(
						"UPDATE [internal].[Users] " +
						"SET Pin = '" + pin + "'"
					);
        }
        
        public override void Down()
        {
        }
    }
}
