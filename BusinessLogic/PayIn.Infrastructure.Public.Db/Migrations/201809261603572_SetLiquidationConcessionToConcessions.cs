namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetLiquidationConcessionToConcessions : DbMigration
    {
        public override void Up()
        {
            Sql(
                "UPDATE PaymentConcessions " +
                        "PaymentConcessions PC INNER JOIN " +
                        "ServiceConcessions SC ON PC.ConcessionId = SC.Id INNER JOIN " +
                        "ServiceSuppliers SS ON SC.SupplierId = SS.Id " +
                        "SS.login = 'info@pay-in.es' " +
                ")"
            );
        }
        
        public override void Down()
        {
        }
    }
}