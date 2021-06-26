namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.Logs", "Duration", c => c.Time(nullable: false, precision: 7));
            AddColumn("internal.Logs", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.Logs", "Login");
            DropColumn("internal.Logs", "Duration");
        }
    }
}
