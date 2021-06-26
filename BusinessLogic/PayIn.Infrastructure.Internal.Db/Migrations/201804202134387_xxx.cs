namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.Logs", "ClientId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.Logs", "ClientId");
        }
    }
}
