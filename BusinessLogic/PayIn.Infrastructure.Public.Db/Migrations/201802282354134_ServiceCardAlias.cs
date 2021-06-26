namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCardAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "Alias", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCards", "Alias");
        }
    }
}
