namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreationPaymentMediaUrls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "OnPaymentMediaCreatedUrl", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.PaymentConcessions", "OnPaymentMediaErrorCreatedUrl", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "OnPaymentMediaErrorCreatedUrl");
            DropColumn("dbo.PaymentConcessions", "OnPaymentMediaCreatedUrl");
        }
    }
}
