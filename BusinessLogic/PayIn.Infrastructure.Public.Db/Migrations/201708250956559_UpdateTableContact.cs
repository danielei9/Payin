namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableContact : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Contacts", new[] { "EntranceId" });
            RenameColumn(table: "dbo.Contacts", name: "EntranceId", newName: "VisitorEntranceId");
            AddColumn("dbo.Contacts", "VisitorLogin", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "VisitorEntranceId", c => c.Int());
            CreateIndex("dbo.Contacts", "VisitorEntranceId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contacts", new[] { "VisitorEntranceId" });
            AlterColumn("dbo.Contacts", "VisitorEntranceId", c => c.Int(nullable: false));
            DropColumn("dbo.Contacts", "VisitorLogin");
            RenameColumn(table: "dbo.Contacts", name: "VisitorEntranceId", newName: "EntranceId");
            CreateIndex("dbo.Contacts", "EntranceId");
        }
    }
}
