namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commercePhotoUrl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventImages", "PhotoUrl", c => c.String(nullable: false));
            DropColumn("dbo.ServiceConcessions", "PhotoUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceConcessions", "PhotoUrl", c => c.String(nullable: false));
            AlterColumn("dbo.EventImages", "PhotoUrl", c => c.String());
        }
    }
}
