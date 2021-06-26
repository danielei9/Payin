namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPaymentConcessionLoginToPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.PaymentMedias", "PaymentConcessionLogin", c => c.String(nullable: false, defaultValue:""));
        }
        
        public override void Down()
        {
            DropColumn("internal.PaymentMedias", "PaymentConcessionLogin");
        }
    }
}
