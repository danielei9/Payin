namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRefactoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceSuppliers", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceSuppliers", "Login");
        }
    }
}
