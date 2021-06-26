namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConcessionStateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceConcessions", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceConcessions", "State");
        }
    }
}
