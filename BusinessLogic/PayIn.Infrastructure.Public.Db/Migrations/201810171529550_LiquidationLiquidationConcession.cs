namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidationLiquidationConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Liquidations", "LiquidationConcessionId", c => c.Int());
            CreateIndex("dbo.Liquidations", "LiquidationConcessionId");
            AddForeignKey("dbo.Liquidations", "LiquidationConcessionId", "dbo.PaymentConcessions", "Id");

            Sql(
                "UPDATE dbo.Liquidations " +
                "SET LiquidationConcessionId = PC.Id " +
                "FROM " +
                    "dbo.PaymentConcessions PC INNER JOIN " +
                    "dbo.ServiceConcessions SC ON PC.ConcessionId=SC.Id INNER JOIN " +
                    "dbo.ServiceSuppliers SS ON SC.SupplierId=SS.Id " +
                "WHERE SS.login='info@pay-in.es'"
            );

            Sql(
                "UPDATE L " +
                "SET LiquidationConcessionId = PC2.id " +
                "FROM " +
                    "dbo.Liquidations L INNER JOIN " +
                    "dbo.PaymentConcessions PC ON L.Concessionid = PC.Id INNER JOIN " +
                    "dbo.ServiceConcessions SC ON PC.ConcessionId = SC.Id INNER JOIN " +
                    "dbo.ServiceSuppliers SS ON SC.SupplierId = SS.Id INNER JOIN " +
                    "dbo.SystemCardMembers SCM ON SCM.login = SS.login INNER JOIN " +
                    "dbo.SystemCards SC2 ON SC2.id = SCM.systemcardid INNER JOIN " +
                    "dbo.PaymentConcessions PC2 ON PC2.Concessionid = SC2.ConcessionOwnerId"
            );
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Liquidations", "LiquidationConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.Liquidations", new[] { "LiquidationConcessionId" });
            DropColumn("dbo.Liquidations", "LiquidationConcessionId");
        }
    }
}
