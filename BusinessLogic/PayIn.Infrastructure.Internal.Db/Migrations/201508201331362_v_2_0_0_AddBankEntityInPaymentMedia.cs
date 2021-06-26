namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_2_0_0_AddBankEntityInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.PaymentMedias", "BankEntity", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.PaymentMedias", "BankEntity");
        }
    }
}
