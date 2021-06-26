namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v_4_1_0_AddData : DbMigration
	{
		public override void Up()
		{
			Sql(
				"UPDATE [dbo].[Transporttitles] SET OperateByPayIn = 0 " +
				"UPDATE [dbo].[Transporttitles] SET OperateByPayIn = 1 WHERE Code in (1552, 1568, 1824, 96, 1808, 1003, 1271, 1272, 1273, 1274, 1275, 1276, 1277, 65535)"
				);
			//Datos para la TuiN
			Sql(
				//Añadiendo soportes
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,1,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,2,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,3,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,4,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,5,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,6,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,9,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,10,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,12,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',2,13,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',10,1,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',10,2,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',10,3,1,1) " +
				"INSERT INTO [dbo].[TransportCardSupports] ([Name],[OwnerCode],[OwnerName],[Type],[SubType],[State],[UsefulWhenExpired]) VALUES ('Tarjeta TuiN',2,'FGV',10,4,1,1) " +
				//Añadiendo compatibilidades
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1271),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 1  and SubType = 1)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1271),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 1  and SubType = 3)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 1)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 2)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 3)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 4)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 5)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 6)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 9)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1274),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 10)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1277),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 12)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1277),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 2  and SubType = 13)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1272),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 10  and SubType = 1)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1273),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 10  and SubType = 2)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1275),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 10  and SubType = 3)) " +
				"INSERT INTO [dbo].[TransportCardSupportTitleCompatibilities] ([TransportTitleId],[TransportCardSupportId]) VALUES ((SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1276),(SELECT Id FROM [dbo].[TransportCardSupports] WHERE OwnerCode = 2 and Type = 10  and SubType = 4))"
				);
			Sql(
				//Modificación del precio del Bono Transbordo AB
				"UPDATE [dbo].[TransportPrices] SET Price = 15.50 WHERE TransportTitleId = (SELECT id FROM dbo.transporttitles WHERE Code = 1552) and Zone = 3 "
				);

			Sql(
				"UPDATE [dbo].[TransportTitles] SET TemporalUnit = null WHERE Id = (SELECT Id FROM [dbo].[TransportTitles] WHERE Code = 1552) " +
				//Actualización de zonas
				"UPDATE[Dbo].[TransportTitles] SET HasZone = 1 WHERE Code = 1824"
				);
		}

		public override void Down()
		{
		}
	}
}
