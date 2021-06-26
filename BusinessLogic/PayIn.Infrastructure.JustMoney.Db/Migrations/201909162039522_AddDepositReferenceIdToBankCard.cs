namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepositReferenceIdToBankCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.BankCardTransactions", "DepositReferenceId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("JustMoney.BankCardTransactions", "DepositReferenceId");
        }
    }
}
