namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStateToProvinceInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.Users", "Province", c => c.String(nullable: false));
            AlterColumn("JustMoney.Users", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("JustMoney.Users", "State", c => c.String(nullable: false));
            DropColumn("JustMoney.Users", "Province");
        }
    }
}
