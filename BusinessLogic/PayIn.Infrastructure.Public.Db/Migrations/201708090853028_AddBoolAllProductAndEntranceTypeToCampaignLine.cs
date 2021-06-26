namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoolAllProductAndEntranceTypeToCampaignLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignLines", "AllProduct", c => c.Boolean(nullable: false, defaultValue:false));
            AddColumn("dbo.CampaignLines", "AllEntranceType", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignLines", "AllEntranceType");
            DropColumn("dbo.CampaignLines", "AllProduct");
        }
    }
}
