namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_2_0_0_AddBankEntityInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMedias", "BankEntity", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentMedias", "BankEntity");
        }
    }
}
