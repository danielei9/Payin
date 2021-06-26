namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceTypeMaxEntrancesNoNullableUseIntMaxValue : DbMigration
    {
        public override void Up()
        {
			Sql("UPDATE dbo.EntranceTypes SET MaxEntrance = 0 WHERE MaxEntrance IS NULL");
            AlterColumn("dbo.EntranceTypes", "MaxEntrance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EntranceTypes", "MaxEntrance", c => c.Int());
        }
    }
}
