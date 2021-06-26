namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_8_ChangeOperatorContextToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportPrices", "OperatorContext", c => c.Long());
			Sql(
				"UPDATE [dbo].[TransportPrices] " +
				"SET OperatorContext = 111 " +
				"WHERE transporttitleid = " + 
						"(SELECT id " + 
						"FROM dbo.transporttitles " +
						"WHERE Code = 1552)"
				);
			Sql(
				"UPDATE [dbo].[TransportPrices] " +
				"SET OperatorContext = 111010111111111 " +
				"WHERE transporttitleid = " +
						"(SELECT id " +
						"FROM dbo.transporttitles " +
						"WHERE Code = 1568)"
				);
			Sql(
				"UPDATE [dbo].[TransportPrices] " +
				"SET OperatorContext = 111010111111111 " +
				"WHERE transporttitleid = " +
						"(SELECT id " +
						"FROM dbo.transporttitles " +
						"WHERE Code = 1824)"
				);
		}
        
        public override void Down()
        {
            AlterColumn("dbo.TransportPrices", "OperatorContext", c => c.Int());
			Sql(
				"UPDATE [dbo].[TransportPrices] " +
				"SET OperatorContext = 0 " +
				"WHERE transporttitleid = " +
						"(SELECT id " +
						"FROM dbo.transporttitles " +
						"WHERE Code = 1568)"
				);
			Sql(
				"UPDATE [dbo].[TransportPrices] " +
				"SET OperatorContext = 0 " +
				"WHERE transporttitleid = " +
						"(SELECT id " +
						"FROM dbo.transporttitles " +
						"WHERE Code = 1824)"
				);

		}
    }
}
