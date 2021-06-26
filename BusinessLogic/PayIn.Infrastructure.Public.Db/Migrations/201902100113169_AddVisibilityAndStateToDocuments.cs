namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVisibilityAndStateToDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceDocuments", "State", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceDocuments", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceDocuments", "Visibility", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceDocuments", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ServiceDocuments", "Url", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceDocuments", "Url", c => c.String());
            AlterColumn("dbo.ServiceDocuments", "Name", c => c.String());
            DropColumn("dbo.ServiceDocuments", "Visibility");
            DropColumn("dbo.ServiceDocuments", "IsVisible");
            DropColumn("dbo.ServiceDocuments", "State");
        }
    }
}
