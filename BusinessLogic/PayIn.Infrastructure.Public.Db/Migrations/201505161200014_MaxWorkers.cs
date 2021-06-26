namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxWorkers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceConcessions", "MaxWorkers", c => c.Int(nullable: false, defaultValue: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceConcessions", "MaxWorkers");
        }
    }
}
