namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCanHasPublicEventToPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "CanHasPublicEvent", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "CanHasPublicEvent");
        }
    }
}
