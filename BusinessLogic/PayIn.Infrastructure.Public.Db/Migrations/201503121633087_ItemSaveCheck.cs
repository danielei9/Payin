namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemSaveCheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlItems", "SaveTrack", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlItems", "SaveTrack");
        }
    }
}
