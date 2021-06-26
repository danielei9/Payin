namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "internal.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        RelatedClass = c.String(nullable: false),
                        RelatedMethod = c.String(nullable: false),
                        RelatedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "internal.LogArguments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        LogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("internal.Logs", t => t.LogId)
                .Index(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("internal.LogArguments", "LogId", "internal.Logs");
            DropIndex("internal.LogArguments", new[] { "LogId" });
            DropTable("internal.LogArguments");
            DropTable("internal.Logs");
        }
    }
}
