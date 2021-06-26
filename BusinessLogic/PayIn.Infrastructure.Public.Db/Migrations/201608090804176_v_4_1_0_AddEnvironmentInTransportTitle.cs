namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddEnvironmentInTransportTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "Environment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "Environment");
        }
    }
}
