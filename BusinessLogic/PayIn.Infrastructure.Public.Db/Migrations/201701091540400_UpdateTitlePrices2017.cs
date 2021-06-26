namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTitlePrices2017 : DbMigration
    {
        public override void Up()
        {
			Sql(
				// Actualización hora de los títulos antiguos
				"UPDATE[dbo].[TransportPrices] SET[End] = '20161231 23:59:00' WHERE TransportTitleId in (SELECT Id FROM[dbo].[TransportTitles] WHERE OperateByPayIn = 0) " +
				//Anulando título antiguo de bonobús
				"UPDATE [dbo].[TransportTitles] SET OperateByPayIn = 0 WHERE Code = 1808 " +
				//Nuevos datos
				"UPDATE [dbo].[TransportPrices] SET [End] = '20171231 23:59:00' WHERE TransportTitleId in (SELECT Id FROM [dbo].[TransportTitles] WHERE OwnerCode in(1,2) and OperateByPayIn = 1) " +
				"UPDATE[dbo].[TransportPrices] SET[End] = '20171231 23:59:00' WHERE TransportTitleId = (SELECT Id FROM[dbo].[TransportTitles] WHERE Code = 96 and version = 5 and OperateByPayIn = 1) " +
				"INSERT INTO[dbo].[TransportPrices] ([Version],[Price],[TransportTitleId],[Start],[End],[Zone],[State],[MaxTimeChanges],[OperatorContext]) VALUES(5,8.5,(SELECT Id FROM[dbo].[TransportTitles] WHERE Code = 96),'20170101','20171231',1,1,60,4)" +
				"UPDATE [dbo].[TransportPrices] SET [End] = '20171231 23:59:00' WHERE TransportTitleId in (SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1003 and OperateByPayIn = 1 and Version = 4)"
				);
        }
        
        public override void Down()
        {
        }
    }
}
