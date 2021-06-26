namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddStateInTransportSystem : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.TransportSystems", "State", c => c.Int(nullable: true));
			Sql(
				"UPDATE dbo.TransportSystems " +
				"SET State = 1"
				);
			AlterColumn("dbo.TransportSystems", "State", c => c.Int(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.TransportSystems", "State");
        }
    }
}
