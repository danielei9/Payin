namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentConcessionMissspelling : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaymentsConcessions", newName: "PaymentConcessions");
            DropColumn("dbo.Tickets", "Currency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "Currency", c => c.String(nullable: false, maxLength: 3));
            RenameTable(name: "dbo.PaymentConcessions", newName: "PaymentsConcessions");
        }
    }
}
