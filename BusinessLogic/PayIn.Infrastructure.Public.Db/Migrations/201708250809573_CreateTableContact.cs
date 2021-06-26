namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExhibitorId = c.Int(nullable: false),
                        EntranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exhibitors", t => t.ExhibitorId)
                .ForeignKey("dbo.Entrances", t => t.EntranceId)
                .Index(t => t.ExhibitorId)
                .Index(t => t.EntranceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "EntranceId", "dbo.Entrances");
            DropForeignKey("dbo.Contacts", "ExhibitorId", "dbo.Exhibitors");
            DropIndex("dbo.Contacts", new[] { "EntranceId" });
            DropIndex("dbo.Contacts", new[] { "ExhibitorId" });
            DropTable("dbo.Contacts");
        }
    }
}
