namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_LogErrorMissatge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Error", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Error");
        }
    }
}
