namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankCardCreatingUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.BankCards", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("JustMoney.BankCards", "RegistrationMessageId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("JustMoney.BankCards", "RegistrationMessageId");
            DropColumn("JustMoney.BankCards", "CreatedAt");
        }
    }
}
