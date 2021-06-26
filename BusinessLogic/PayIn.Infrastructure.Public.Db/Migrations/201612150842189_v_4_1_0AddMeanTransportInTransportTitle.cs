namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0AddMeanTransportInTransportTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "MeanTransport", c => c.Int(nullable:true));
			Sql(
				"UPDATE [dbo].[TransportTitles] SET MeanTransport = 0 where code = 96 " +
				"UPDATE [dbo].[TransportTitles] SET MeanTransport = 1 where code in (1808, 1003, 1271, 1272, 1273, 1274, 1275, 1276, 1277)" +
			    "UPDATE [dbo].[TransportTitles] SET MeanTransport = 2 where code in (1824,1568,1552) "
				);
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "MeanTransport");
        }
    }
}
