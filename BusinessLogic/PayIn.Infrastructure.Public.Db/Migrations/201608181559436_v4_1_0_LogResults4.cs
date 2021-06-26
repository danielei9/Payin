namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_LogResults4 : DbMigration
    {
        public override void Up()
		{
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=20000 AND id<21000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=21000 AND id<22000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=22000 AND id<24000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=24000 AND id<26000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=26000 AND id<28000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=28000 AND id<30000");
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id>=30000 AND id<32000");
		}
        
        public override void Down()
        {
        }
    }
}
