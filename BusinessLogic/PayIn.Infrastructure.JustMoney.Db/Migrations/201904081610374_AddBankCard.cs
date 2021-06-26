namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBankCard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "JustMoney.BankCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Login = c.String(nullable: false),
                        Token = c.String(nullable: false),
                        Pan = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        ExpirationMM = c.String(nullable: false),
                        ExpirationYYYY = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("JustMoney.BankCards");
        }
    }
}
