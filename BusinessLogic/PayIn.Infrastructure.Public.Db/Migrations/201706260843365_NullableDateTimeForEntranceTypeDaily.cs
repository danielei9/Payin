namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateTimeForEntranceTypeDaily : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EntranceTypes", "StartDay", c => c.DateTime(defaultValue:null));
            AlterColumn("dbo.EntranceTypes", "EndDay", c => c.DateTime(defaultValue: null));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EntranceTypes", "EndDay", c => c.DateTime(nullable: false, defaultValue: DateTime.MinValue));
            AlterColumn("dbo.EntranceTypes", "StartDay", c => c.DateTime(nullable: false, defaultValue: DateTime.MaxValue));
        }
    }
}
