namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentConcessionUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "Observations", c => c.String());
            AddColumn("dbo.PaymentConcessions", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.PaymentConcessions", "FormA", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "FormA");
            DropColumn("dbo.PaymentConcessions", "Phone");
            DropColumn("dbo.PaymentConcessions", "Observations");
        }
    }
}
