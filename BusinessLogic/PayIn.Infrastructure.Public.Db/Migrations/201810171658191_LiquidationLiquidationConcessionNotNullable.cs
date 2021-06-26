namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidationLiquidationConcessionNotNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Liquidations", new[] { "LiquidationConcessionId" });
            AlterColumn("dbo.Liquidations", "LiquidationConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Liquidations", "LiquidationConcessionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Liquidations", new[] { "LiquidationConcessionId" });
            AlterColumn("dbo.Liquidations", "LiquidationConcessionId", c => c.Int());
            CreateIndex("dbo.Liquidations", "LiquidationConcessionId");
        }
    }
}
