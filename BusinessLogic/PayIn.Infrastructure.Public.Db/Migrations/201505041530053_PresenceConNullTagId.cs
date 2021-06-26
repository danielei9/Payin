namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PresenceConNullTagId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ControlPresences", new[] { "TagId" });
            AlterColumn("dbo.ControlPresences", "TagId", c => c.Int());
            CreateIndex("dbo.ControlPresences", "TagId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ControlPresences", new[] { "TagId" });
            AlterColumn("dbo.ControlPresences", "TagId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlPresences", "TagId");
        }
    }
}
