namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_NoNullableOwnerCodeInTransportTitle : DbMigration
    {
        public override void Up()
		{
			AlterColumn("dbo.TransportTitles", "OwnerCode", c => c.Int(nullable: true));
			Sql(
							"UPDATE dbo.TransportTitles " +
							"SET " +
								"OwnerCode = 0 " +
							"WHERE dbo.TransportTitles.OwnerCode IS NULL"
						);
			AlterColumn("dbo.TransportTitles", "OwnerCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportTitles", "OwnerCode", c => c.Int());
        }
    }
}
