namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Duration", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Logs", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Login");
            DropColumn("dbo.Logs", "Duration");
        }
    }
}
