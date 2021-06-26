namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientIdInLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "ClientId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "ClientId");
        }
    }
}
