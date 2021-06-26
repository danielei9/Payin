namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceDomain_PlatformDomain : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Devices", new[] { "Platform_Id" });
            RenameColumn(table: "dbo.Devices", name: "Platform_Id", newName: "PlatformId");
            AlterColumn("dbo.Devices", "PlatformId", c => c.Int(nullable: false));
            CreateIndex("dbo.Devices", "PlatformId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Devices", new[] { "PlatformId" });
            AlterColumn("dbo.Devices", "PlatformId", c => c.Int());
            RenameColumn(table: "dbo.Devices", name: "PlatformId", newName: "Platform_Id");
            CreateIndex("dbo.Devices", "Platform_Id");
        }
    }
}
