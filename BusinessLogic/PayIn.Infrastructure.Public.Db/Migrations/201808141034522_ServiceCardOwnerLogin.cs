namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCardOwnerLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "OwnerLogin", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCards", "OwnerLogin");
        }
    }
}
