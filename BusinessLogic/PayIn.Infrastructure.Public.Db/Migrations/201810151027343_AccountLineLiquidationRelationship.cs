namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountLineLiquidationRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountLines", "LiquidationId", c => c.Int());
            CreateIndex("dbo.AccountLines", "LiquidationId");
            AddForeignKey("dbo.AccountLines", "LiquidationId", "dbo.Liquidations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountLines", "LiquidationId", "dbo.Liquidations");
            DropIndex("dbo.AccountLines", new[] { "LiquidationId" });
            DropColumn("dbo.AccountLines", "LiquidationId");
        }
    }
}
