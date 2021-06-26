namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_LogResults2 : DbMigration
    {
        public override void Up()
        {
			Sql("INSERT dbo.LogResults (Name, Value, LogId) SELECT 'operationId', result, id FROM dbo.Logs WHERE result != '' AND id<10000"); 
		}
        
        public override void Down()
        {
        }
    }
}
