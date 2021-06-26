namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBankCardToTransaction : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "JustMoney.BankCards", newName: "BankCardTransactions");
            AddColumn("JustMoney.BankCardTransactions", "CardHolderId", c => c.String(nullable: false));
            AddColumn("JustMoney.BankCardTransactions", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("JustMoney.BankCardTransactions", "Amount");
            DropColumn("JustMoney.BankCardTransactions", "CardHolderId");
            RenameTable(name: "JustMoney.BankCardTransactions", newName: "BankCards");
        }
    }
}
