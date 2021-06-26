namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableContact1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contacts", "EventId");
            AddForeignKey("dbo.Contacts", "EventId", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "EventId", "dbo.Events");
            DropIndex("dbo.Contacts", new[] { "EventId" });
            DropColumn("dbo.Contacts", "EventId");
        }
    }
}
