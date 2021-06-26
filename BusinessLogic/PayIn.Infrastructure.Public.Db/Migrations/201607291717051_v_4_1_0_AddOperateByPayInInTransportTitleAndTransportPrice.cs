namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddOperateByPayInInTransportTitleAndTransportPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "OperateByPayIn", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.TransportPrices", "OperateByPayIn", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportPrices", "OperateByPayIn");
            DropColumn("dbo.TransportTitles", "OperateByPayIn");
        }
    }
}
