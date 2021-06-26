namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "JustMoney.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        Login = c.String(nullable: false),
                        ClientId = c.String(nullable: false),
                        RelatedClass = c.String(nullable: false),
                        RelatedMethod = c.String(nullable: false),
                        RelatedId = c.Long(nullable: false),
                        Error = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "JustMoney.LogArguments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        LogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("JustMoney.Logs", t => t.LogId)
                .Index(t => t.LogId);
            
            CreateTable(
                "JustMoney.LogResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        LogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("JustMoney.Logs", t => t.LogId)
                .Index(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("JustMoney.LogResults", "LogId", "JustMoney.Logs");
            DropForeignKey("JustMoney.LogArguments", "LogId", "JustMoney.Logs");
            DropIndex("JustMoney.LogResults", new[] { "LogId" });
            DropIndex("JustMoney.LogArguments", new[] { "LogId" });
            DropTable("JustMoney.LogResults");
            DropTable("JustMoney.LogArguments");
            DropTable("JustMoney.Logs");
        }
    }
}
