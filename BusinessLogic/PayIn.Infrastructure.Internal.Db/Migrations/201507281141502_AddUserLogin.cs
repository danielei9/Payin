namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.Users", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.Users", "Login");
        }
    }
}
