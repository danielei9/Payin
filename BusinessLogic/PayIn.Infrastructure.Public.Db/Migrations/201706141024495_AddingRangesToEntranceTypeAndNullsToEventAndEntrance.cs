namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRangesToEntranceTypeAndNullsToEventAndEntrance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "RangeMin", c => c.Int());
            AddColumn("dbo.EntranceTypes", "RangeMax", c => c.Int());
            AlterColumn("dbo.Events", "Code", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Code", c => c.Long(nullable: false));
            DropColumn("dbo.EntranceTypes", "RangeMax");
            DropColumn("dbo.EntranceTypes", "RangeMin");
        }
    }
}
