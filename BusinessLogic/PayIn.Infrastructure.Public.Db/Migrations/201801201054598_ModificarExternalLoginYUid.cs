namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificarExternalLoginYUid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "ExternalLogin", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Uid", c => c.String(nullable: false));
            DropColumn("dbo.Tickets", "ExternalLogin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "ExternalLogin", c => c.String(nullable: false));
            DropColumn("dbo.Payments", "Uid");
            DropColumn("dbo.Payments", "ExternalLogin");
        }
    }
}
