namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntranceFromValue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntranceFormValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormValueId = c.Int(nullable: false),
                        EntranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entrances", t => t.EntranceId)
                .Index(t => t.EntranceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntranceFormValues", "EntranceId", "dbo.Entrances");
            DropIndex("dbo.EntranceFormValues", new[] { "EntranceId" });
            DropTable("dbo.EntranceFormValues");
        }
    }
}
