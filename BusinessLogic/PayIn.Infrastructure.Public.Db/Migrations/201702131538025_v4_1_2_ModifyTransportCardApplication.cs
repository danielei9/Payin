namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_2_ModifyTransportCardApplication : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportCardApplications", "ReadKey", c => c.Int());
            AlterColumn("dbo.TransportCardApplications", "WriteKey", c => c.Int());
			Sql(
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES ('00',1,'FB38A95E34880400465944135D104408,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM [dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('01', 1, '3110C0D20000000000200000000040D8,010000000A000000000000000000004C,010000000A000000000000000000004C', 'FF078069', 0, 1, (SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic')) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('02',1,'0000000018A83083559A00842D010070,20202020202020202020202020200048,808080808000030000000000000018A4','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " + 
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('03',1,'01C200A0074000000020FCDFDF04004D,0A77E82A0D000000000000000000008D,2020202020202020202020202020206B','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('04',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('05',1,'00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('06',1,'00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('07',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('08',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('09',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0A',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0B',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0C',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0D',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0E',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' )) " +
				"INSERT INTO [dbo].[TransportCardApplications] ([ApplicationId],[KeyVersion],[Content],[AccessCondition],[ReadKey],[WriteKey],[TransportSystemId]) VALUES('0F',1,'000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1','FF078069',0,1,(SELECT Id FROM[dbo].[TransportSystems] WHERE Name like 'Valencia Classic' ))"
				);
		}
        
        public override void Down()
        {
            AlterColumn("dbo.TransportCardApplications", "WriteKey", c => c.Int(nullable: false));
            AlterColumn("dbo.TransportCardApplications", "ReadKey", c => c.Int(nullable: false));
		}
    }
}
