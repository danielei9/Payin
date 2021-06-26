namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxEntrancesPerCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "MaxEntrancesPerCard", c => c.Int(nullable: false, defaultValue: int.MaxValue));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "MaxEntrancesPerCard");
        }
    }
}
