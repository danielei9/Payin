namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceChekpointFormAssign : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ControlFormAssigns", new[] { "CheckId" });
            AddColumn("dbo.ControlFormAssigns", "CheckPointId", c => c.Int());
            AlterColumn("dbo.ControlFormAssigns", "CheckId", c => c.Int());
            CreateIndex("dbo.ControlFormAssigns", "CheckId");
            CreateIndex("dbo.ControlFormAssigns", "CheckPointId");
            AddForeignKey("dbo.ControlFormAssigns", "CheckPointId", "dbo.ServiceCheckPoints", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormAssigns", "CheckPointId", "dbo.ServiceCheckPoints");
            DropIndex("dbo.ControlFormAssigns", new[] { "CheckPointId" });
            DropIndex("dbo.ControlFormAssigns", new[] { "CheckId" });
            AlterColumn("dbo.ControlFormAssigns", "CheckId", c => c.Int(nullable: false));
            DropColumn("dbo.ControlFormAssigns", "CheckPointId");
            CreateIndex("dbo.ControlFormAssigns", "CheckId");
        }
    }
}
