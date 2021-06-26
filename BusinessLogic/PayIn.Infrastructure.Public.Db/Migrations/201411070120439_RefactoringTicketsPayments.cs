namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringTicketsPayments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.Payments", new[] { "SupplierId" });
            AddColumn("dbo.Payments", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payments", "Until", c => c.DateTime());
            AddColumn("dbo.Tickets", "Until", c => c.DateTime());
            AddColumn("dbo.Tickets", "TaxName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "TaxAddress", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "TaxNumber", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierId", c => c.Int(nullable: true));

						Sql(
							"UPDATE dbo.Tickets " +
							"SET " +
								"SupplierId = P.SupplierId, " +
								"Date = P.Date " +
							"FROM " +
								"dbo.Payments P " +
							"WHERE P.ticketId = dbo.Tickets.id"
						);
						Sql(
							"UPDATE dbo.Tickets " +
							"SET " +
								"SupplierId = 1 " +
							"WHERE dbo.Tickets.SupplierId IS NULL"
						);

						AlterColumn("dbo.Tickets", "SupplierId", c => c.Int(nullable: false));
						CreateIndex("dbo.Tickets", "SupplierId");
						AddForeignKey("dbo.Tickets", "SupplierId", "dbo.ServiceSuppliers", "Id");

						DropColumn("dbo.Payments", "SupplierId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "SupplierId", c => c.Int(nullable: false));

						Sql(
							"UPDATE dbo.Payments " +
							"SET SupplierId = T.SupplierId " +
							"FROM " +
								"dbo.Tickets T " +
							"WHERE dbo.Payments.ticketId = T.id");

            DropForeignKey("dbo.Tickets", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.Tickets", new[] { "SupplierId" });
            DropColumn("dbo.Tickets", "SupplierId");
            DropColumn("dbo.Tickets", "TaxNumber");
            DropColumn("dbo.Tickets", "TaxAddress");
            DropColumn("dbo.Tickets", "TaxName");
            DropColumn("dbo.Tickets", "Until");
            DropColumn("dbo.Payments", "Until");
            DropColumn("dbo.Payments", "Date");
            CreateIndex("dbo.Payments", "SupplierId");
            AddForeignKey("dbo.Payments", "SupplierId", "dbo.ServiceSuppliers", "Id");
        }
    }
}
