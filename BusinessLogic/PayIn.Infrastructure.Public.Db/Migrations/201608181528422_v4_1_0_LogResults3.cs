namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_LogResults3 : DbMigration
    {
        public override void Up()
		{
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=10000 AND id<12000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=12000 AND id<14000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=14000 AND id<16000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=16000 AND id<18000");
			//Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=18000 AND id<20000");
		}

		public override void Down()
        {
			//
		}
    }
}
