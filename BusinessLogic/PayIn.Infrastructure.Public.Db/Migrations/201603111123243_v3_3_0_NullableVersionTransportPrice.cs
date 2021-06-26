namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_NullableVersionTransportPrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportPrices", "Version", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportPrices", "Version", c => c.Int(nullable: false));
        }
    }
}
