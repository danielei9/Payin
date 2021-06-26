namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUserNameInPrepaidCardAndAddAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.PrepaidCards", "Alias", c => c.String(nullable: false));
            DropColumn("JustMoney.PrepaidCards", "FirstName");
            DropColumn("JustMoney.PrepaidCards", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("JustMoney.PrepaidCards", "LastName", c => c.String(nullable: false));
            AddColumn("JustMoney.PrepaidCards", "FirstName", c => c.String(nullable: false));
            DropColumn("JustMoney.PrepaidCards", "Alias");
        }
    }
}
