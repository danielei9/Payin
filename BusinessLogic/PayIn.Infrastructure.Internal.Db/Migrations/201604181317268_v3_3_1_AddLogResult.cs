namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_1_AddLogResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.Logs", "Error", c => c.String(nullable: false));
            AddColumn("internal.Logs", "Result", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.Logs", "Result");
            DropColumn("internal.Logs", "Error");
        }
    }
}
