namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOnlineCartActivatedtoPaymentConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "OnlineCartActivated", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "OnlineCartActivated");
        }
    }
}
