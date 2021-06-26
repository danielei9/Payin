namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_LogResults5 : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.Logs", "Result");
		}
        
        public override void Down()
        {
			AddColumn("dbo.Logs", "Result", c => c.String(nullable: false));
		}
    }
}
