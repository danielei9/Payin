namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddStateInTransportTitle : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.TransportTitles", "State", c => c.Int(nullable: true));
			Sql(
				"UPDATE dbo.TransportTitles " +
				"SET State = 1"
				);
			AlterColumn("dbo.TransportTitles", "State", c => c.Int(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "State");
        }
    }
}
