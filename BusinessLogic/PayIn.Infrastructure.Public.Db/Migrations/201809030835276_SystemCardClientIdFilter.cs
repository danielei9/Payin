namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemCardClientIdFilter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCards", "ClientId", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCards", "ClientId");
        }
    }
}
