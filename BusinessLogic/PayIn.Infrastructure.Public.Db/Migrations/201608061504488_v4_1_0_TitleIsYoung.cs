namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_TitleIsYoung : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "IsYoung", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "IsYoung");
        }
    }
}
