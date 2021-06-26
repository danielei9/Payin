namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceEnttyDomain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Login = c.String(),
                        Platform_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Platforms", t => t.Platform_Id)
                .Index(t => t.Platform_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "Platform_Id", "dbo.Platforms");
            DropIndex("dbo.Devices", new[] { "Platform_Id" });
            DropTable("dbo.Devices");
        }
    }
}
