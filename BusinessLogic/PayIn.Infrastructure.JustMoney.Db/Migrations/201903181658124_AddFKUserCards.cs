namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKUserCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.PrepaidCards", "UserId", c => c.Int());
            CreateIndex("JustMoney.PrepaidCards", "UserId");
            AddForeignKey("JustMoney.PrepaidCards", "UserId", "JustMoney.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("JustMoney.PrepaidCards", "UserId", "JustMoney.Users");
            DropIndex("JustMoney.PrepaidCards", new[] { "UserId" });
            DropColumn("JustMoney.PrepaidCards", "UserId");
        }
    }
}
