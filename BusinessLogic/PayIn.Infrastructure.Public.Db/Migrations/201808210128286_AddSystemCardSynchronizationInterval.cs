namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemCardSynchronizationInterval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCards", "SynchronizationInterval", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCards", "SynchronizationInterval");
        }
    }
}
