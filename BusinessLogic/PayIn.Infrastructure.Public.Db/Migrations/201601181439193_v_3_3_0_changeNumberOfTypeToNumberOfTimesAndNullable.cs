namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_3_0_changeNumberOfTypeToNumberOfTimesAndNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "NumberOfTimes", c => c.Int(nullable: true));
            DropColumn("dbo.Campaigns", "NumberOfType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "NumberOfType", c => c.Int(nullable: false));
            DropColumn("dbo.Campaigns", "NumberOfTimes");
        }
    }
}
