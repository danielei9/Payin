namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_PaymentConcessionModify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "Address", c => c.String(nullable: true));
			Sql(
				"UPDATE PC " +
				"SET Address = SS.TaxAddress " +
				"FROM " + 
					"dbo.PaymentConcessions PC INNER JOIN " +
					"dbo.ServiceConcessions SC ON SC.Id = PC.ConcessionId INNER JOIN " +
					"dbo.ServiceSuppliers SS ON SS.Id = SC.SupplierId ");
			AlterColumn("dbo.PaymentConcessions", "Address", c => c.String(nullable: false));
            AddColumn("dbo.PaymentConcessions", "CreateConcessionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "CreateConcessionDate");
            DropColumn("dbo.PaymentConcessions", "Address");
        }
    }
}
