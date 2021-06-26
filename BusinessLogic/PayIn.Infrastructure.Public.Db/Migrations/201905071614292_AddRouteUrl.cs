namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "RouteUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "RouteUrl");
        }
    }
}
