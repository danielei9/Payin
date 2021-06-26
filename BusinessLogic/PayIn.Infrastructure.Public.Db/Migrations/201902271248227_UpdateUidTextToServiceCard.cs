namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUidTextToServiceCard : DbMigration
    {
        public override void Up()
        {
			Sql(
				"UPDATE ServiceCards " +				"SET UidText = ( " +				"	CASE SCB.UidFormat " +				"		WHEN 2 THEN " +				"			FORMAT((SC.Uid / 268435456) % 16, 'X') + " +				"			FORMAT((SC.Uid / 16777216) % 16, 'X') + " +				"			FORMAT((SC.Uid / 1048576) % 16, 'X') + " +				"			FORMAT((SC.Uid / 65536) % 16, 'X') + " +				"			FORMAT((SC.Uid / 4096) % 16, 'X') + " +				"			FORMAT((SC.Uid / 256) % 16, 'X') + " +				"			FORMAT((SC.Uid / 16) % 16, 'X') + " +				"			FORMAT((SC.Uid / 1) % 16, 'X') " +				"		WHEN 1 THEN " +				"			FORMAT((SC.Uid / 16) % 16, 'X') + " +				"			FORMAT((SC.Uid / 1) % 16, 'X') + " +				"			FORMAT((SC.Uid / 4096) % 16, 'X') + " +				"			FORMAT((SC.Uid / 256) % 16, 'X') + " +				"			FORMAT((SC.Uid / 1048576) % 16, 'X') + " +				"			FORMAT((SC.Uid / 65536) % 16, 'X') + " +				"			FORMAT((SC.Uid / 268435456) % 16, 'X') + " +				"			FORMAT((SC.Uid / 16777216) % 16, 'X') " +				"		ELSE CONVERT(NVARCHAR(MAX), SC.Uid) " +				"	END) " +				"FROM " +				"	ServiceCards SC INNER JOIN " +				"	ServiceCardBatches SCB ON SC.ServiceCardBatchId = SCB.Id "
			);
        }
        
        public override void Down()
        {
        }
    }
}
