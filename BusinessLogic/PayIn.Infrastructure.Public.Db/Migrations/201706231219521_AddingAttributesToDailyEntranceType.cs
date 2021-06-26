namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAttributesToDailyEntranceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "NumDay", c => c.Int());
            AddColumn("dbo.EntranceTypes", "StartDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.EntranceTypes", "EndDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntranceTypes", "EndDay");
            DropColumn("dbo.EntranceTypes", "StartDay");
            DropColumn("dbo.EntranceTypes", "NumDay");
        }
    }
}
