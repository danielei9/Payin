namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId2", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId2", c => c.Int(nullable: false));
        }
    }
}
