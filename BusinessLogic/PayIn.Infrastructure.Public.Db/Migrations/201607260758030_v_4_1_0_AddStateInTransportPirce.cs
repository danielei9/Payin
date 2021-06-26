namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddStateInTransportPirce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "State", c => c.Int(nullable: true));
			Sql(
				"UPDATE dbo.TransportPrices " +
				"SET State = 1"
				);
			AlterColumn("dbo.TransportPrices", "State", c => c.Int(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.TransportPrices", "State");
        }
    }
}
