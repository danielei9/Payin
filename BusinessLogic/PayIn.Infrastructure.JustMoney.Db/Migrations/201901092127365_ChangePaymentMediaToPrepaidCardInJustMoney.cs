namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePaymentMediaToPrepaidCardInJustMoney : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "JustMoney.PaymentMedias", newName: "PrepaidCards");
            AddColumn("JustMoney.PrepaidCards", "Name", c => c.String(nullable: false, defaultValue: "Prueba"));
        }
        
        public override void Down()
        {
            DropColumn("JustMoney.PrepaidCards", "Name");
            RenameTable(name: "JustMoney.PrepaidCards", newName: "PaymentMedias");
        }
    }
}
