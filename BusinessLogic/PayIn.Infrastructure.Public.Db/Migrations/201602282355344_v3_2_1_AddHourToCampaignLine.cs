namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_2_1_AddHourToCampaignLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignLines", "SinceTime", c => c.DateTime());
            AddColumn("dbo.CampaignLines", "UntilTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignLines", "UntilTime");
            DropColumn("dbo.CampaignLines", "SinceTime");
        }
    }
}
