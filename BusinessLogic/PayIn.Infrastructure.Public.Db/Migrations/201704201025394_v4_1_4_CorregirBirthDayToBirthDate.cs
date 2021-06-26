namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_4_CorregirBirthDayToBirthDate : DbMigration
    {
        public override void Up()
        {
			RenameColumn("dbo.ServiceUsers", "BirthDay", "BirthDate");
        }
        
        public override void Down()
        {
			RenameColumn("dbo.ServiceUsers", "BirthDate","BirthDay" );
		}
    }
}
