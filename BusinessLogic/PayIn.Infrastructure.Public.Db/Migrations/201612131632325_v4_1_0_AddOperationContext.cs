namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v4_1_0_AddOperationContext : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.TransportPrices", "OperatorContext", c => c.Long());
			Sql(
				"UPDATE dbo.transportprices SET OperatorContext = 30207 WHERE TransportTitleId = (SELECT id FROM[dbo].[transporttitles] WHERE Code = 1568) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM[dbo].[transporttitles] WHERE Code = 1552 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 30207	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1552 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1552 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1552 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 30207	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1824) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 112) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1648) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1904) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 368) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 624) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 880) " +
				"UPDATE dbo.transportprices SET OperatorContext = 4	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 96) " +
				"UPDATE dbo.transportprices SET OperatorContext = 4	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 128) " +
				//Antiguo Bonometro - en desuso
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1808 and Zone in (1, 2, 4, 8))  " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1808 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1808 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1808 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2080 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2080 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2080 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2080 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2336 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2336 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2336 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2336 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 100000 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2320) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2592 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2592 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2592 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2592 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 2848) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3888 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3888 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3888 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3888 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3968 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3968 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3968 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 2	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3968 and Zone in (15)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3120) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391	WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3376) " +
				"UPDATE dbo.transportprices SET OperatorContext = 16391 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 3632) " +
				"UPDATE dbo.transportprices SET OperatorContext = 18 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1003 and Zone in (1, 2, 4, 8)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 18 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1003 and Zone in (3, 6, 12)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 18 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1003 and Zone in (7, 14)) " +
				"UPDATE dbo.transportprices SET OperatorContext = 18 WHERE TransportTitleId = (SELECT id FROM [dbo].[transporttitles] WHERE Code = 1003 and Zone in (15)) " +			
				"UPDATE [dbo].[TransportPrices] SET OperatorContext = 2 WHERE TransportTitleId in (SELECT Id FROM [dbo].[TransportTitles] WHERE Code in (1271,1272,1273,1274,1275,1276,1277))"
				);
			//DropColumn("dbo.TransportTitles", "OperatorContext");
		}

		public override void Down()
		{
			//AddColumn("dbo.TransportTitles", "OperatorContext", c => c.Long());
			DropColumn("dbo.TransportPrices", "OperatorContext");
		}
	}
}
