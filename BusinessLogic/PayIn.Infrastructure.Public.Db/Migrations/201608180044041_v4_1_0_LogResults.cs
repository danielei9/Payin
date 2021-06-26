namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_LogResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        LogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Logs", t => t.LogId)
                .Index(t => t.LogId);
        }
        
        public override void Down()
        {
			DropForeignKey("dbo.LogResults", "LogId", "dbo.Logs");
            DropIndex("dbo.LogResults", new[] { "LogId" });
            DropTable("dbo.LogResults");
        }
    }
}
