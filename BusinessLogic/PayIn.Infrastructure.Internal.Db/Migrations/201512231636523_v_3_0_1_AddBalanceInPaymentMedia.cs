namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_0_1_AddBalanceInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.PaymentMedias", "Balance", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("internal.PaymentMedias", "Balance");
        }
    }
}
