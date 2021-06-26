namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationTableProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Icon = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        Layout = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "ProfileId", c => c.Int());
            AddColumn("dbo.SystemCards", "ProfileId", c => c.Int());
            CreateIndex("dbo.Events", "ProfileId");
            CreateIndex("dbo.SystemCards", "ProfileId");
            AddForeignKey("dbo.Events", "ProfileId", "dbo.Profiles", "Id");
            AddForeignKey("dbo.SystemCards", "ProfileId", "dbo.Profiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemCards", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Events", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.SystemCards", new[] { "ProfileId" });
            DropIndex("dbo.Events", new[] { "ProfileId" });
            DropColumn("dbo.SystemCards", "ProfileId");
            DropColumn("dbo.Events", "ProfileId");
            DropTable("dbo.Profiles");
        }
    }
}
