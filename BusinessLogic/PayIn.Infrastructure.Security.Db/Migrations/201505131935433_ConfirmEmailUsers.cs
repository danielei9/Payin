namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfirmEmailUsers : DbMigration
    {
        public override void Up()
        {
					Sql("UPDATE [security].[AspNetUsers] SET EmailConfirmed = 1");
        }
        
        public override void Down()
        {
        }
    }
}
