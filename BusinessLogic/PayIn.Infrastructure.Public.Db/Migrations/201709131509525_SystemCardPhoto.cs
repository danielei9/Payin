namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemCardPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCards", "PhotoUrl", c => c.String(nullable: false));

			Sql("UPDATE dbo.SystemCards SET PhotoUrl='https://pbs.twimg.com/media/C9wzMI-XgAA6bGO.jpg'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCards", "PhotoUrl");
        }
    }
}
