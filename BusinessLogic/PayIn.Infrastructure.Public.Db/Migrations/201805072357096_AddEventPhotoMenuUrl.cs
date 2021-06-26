namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventPhotoMenuUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "PhotoMenuUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "PhotoMenuUrl");
        }
    }
}
