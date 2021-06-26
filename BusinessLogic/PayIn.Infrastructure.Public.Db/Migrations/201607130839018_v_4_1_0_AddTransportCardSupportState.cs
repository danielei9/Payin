namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddTransportCardSupportState : DbMigration
    {
        public override void Up()
        {           
			AddColumn("dbo.TransportCardSupports", "State", c => c.Int(nullable: true));
			Sql(
				"UPDATE dbo.TransportCardSupports " +
				"SET State = 1"
				);
			AlterColumn("dbo.TransportCardSupports", "State", c => c.Int(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.TransportCardSupports", "State");
        }
    }
}
