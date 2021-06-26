namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddNullableEnvironmentInTransportTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportTitles", "Environment", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportTitles", "Environment", c => c.Int(nullable: false));
        }
    }
}
