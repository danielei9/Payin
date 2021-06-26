namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSinceAndUntilToDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceDocuments", "Since", c => c.DateTime(nullable: false));
            AddColumn("dbo.ServiceDocuments", "Until", c => c.DateTime(nullable: false));
            DropColumn("dbo.ServiceDocuments", "Start");
            DropColumn("dbo.ServiceDocuments", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceDocuments", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.ServiceDocuments", "Start", c => c.DateTime(nullable: false));
            DropColumn("dbo.ServiceDocuments", "Until");
            DropColumn("dbo.ServiceDocuments", "Since");
        }
    }
}
