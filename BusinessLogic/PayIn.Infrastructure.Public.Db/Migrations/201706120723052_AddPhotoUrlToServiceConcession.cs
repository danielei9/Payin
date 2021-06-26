namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoUrlToServiceConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceConcessions", "PhotoUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceConcessions", "PhotoUrl");
        }
    }
}
