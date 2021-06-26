namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateClassEntranceSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntranceSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Template = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "EntranceSystemId", c => c.Int(nullable: true));

			Sql("INSERT dbo.EntranceSystems (name, type, template) values ('Pay[in]', 0, 'Payin:Entrance {0}')");
			Sql("UPDATE dbo.Events SET EntranceSystemId = (SELECT TOP 1 id FROM dbo.EntranceSystems)");

			AlterColumn("dbo.Events", "EntranceSystemId", c => c.Int(nullable: false));
			CreateIndex("dbo.Events", "EntranceSystemId");
            AddForeignKey("dbo.Events", "EntranceSystemId", "dbo.EntranceSystems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "EntranceSystemId", "dbo.EntranceSystems");
            DropIndex("dbo.Events", new[] { "EntranceSystemId" });
            DropColumn("dbo.Events", "EntranceSystemId");
            DropTable("dbo.EntranceSystems");
        }
    }
}
