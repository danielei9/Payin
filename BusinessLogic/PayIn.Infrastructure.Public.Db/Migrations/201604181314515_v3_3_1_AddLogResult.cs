namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_1_AddLogResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Result", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Result");
        }
    }
}
