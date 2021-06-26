namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTablesEventFormAndEntranceTypeForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntranceTypeForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        EntranceTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntranceTypes", t => t.EntranceTypeId)
                .Index(t => t.EntranceTypeId);
            
            CreateTable(
                "dbo.EventForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventForms", "EventId", "dbo.Events");
            DropForeignKey("dbo.EntranceTypeForms", "EntranceTypeId", "dbo.EntranceTypes");
            DropIndex("dbo.EventForms", new[] { "EventId" });
            DropIndex("dbo.EntranceTypeForms", new[] { "EntranceTypeId" });
            DropTable("dbo.EventForms");
            DropTable("dbo.EntranceTypeForms");
        }
    }
}
