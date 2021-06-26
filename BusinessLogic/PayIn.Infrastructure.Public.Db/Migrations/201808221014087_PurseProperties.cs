namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurseProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purses", "Slot", c => c.Int());
            AddColumn("dbo.Purses", "IsPayin", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Purses", "IsRechargable", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purses", "IsRechargable");
            DropColumn("dbo.Purses", "IsPayin");
            DropColumn("dbo.Purses", "Slot");
        }
    }
}
