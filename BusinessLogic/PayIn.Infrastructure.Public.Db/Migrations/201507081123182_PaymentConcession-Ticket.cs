namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentConcessionTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Vat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Tickets", "IncomeTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PaymentConcessions", "Vat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PaymentConcessions", "IncomeTax", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "IncomeTax");
            DropColumn("dbo.PaymentConcessions", "Vat");
            DropColumn("dbo.Tickets", "IncomeTax");
            DropColumn("dbo.Tickets", "Vat");
        }
    }
}
