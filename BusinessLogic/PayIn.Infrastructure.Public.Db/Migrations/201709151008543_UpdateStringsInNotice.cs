namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStringsInNotice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notices", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Notices", "ShortDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Notices", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Notices", "PhotoUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notices", "PhotoUrl", c => c.String());
            AlterColumn("dbo.Notices", "Description", c => c.String());
            AlterColumn("dbo.Notices", "ShortDescription", c => c.String());
            AlterColumn("dbo.Notices", "Name", c => c.String());
        }
    }
}
