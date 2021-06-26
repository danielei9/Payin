namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_2_AddChecksInLiquidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Liquidations", "PaidBank", c => c.Boolean(nullable: false, defaultValue:false));
            AddColumn("dbo.Liquidations", "PaidTPV", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Liquidations", "PaidTPV");
            DropColumn("dbo.Liquidations", "PaidBank");
        }
    }
}
