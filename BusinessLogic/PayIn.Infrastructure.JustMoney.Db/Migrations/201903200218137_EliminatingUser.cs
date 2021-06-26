namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminatingUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("JustMoney.PrepaidCards", "UserId", "JustMoney.Users");
            DropIndex("JustMoney.PrepaidCards", new[] { "UserId" });
            AddColumn("JustMoney.PrepaidCards", "State", c => c.Int(nullable: false));
            AddColumn("JustMoney.PrepaidCards", "FirstName", c => c.String(nullable: false));
            AddColumn("JustMoney.PrepaidCards", "LastName", c => c.String(nullable: false));
            RenameColumn("JustMoney.PrepaidCards", "InternalToken", "CardHolderId");
            DropColumn("JustMoney.PrepaidCards", "Name");
            DropColumn("JustMoney.PrepaidCards", "UserId");
            DropTable("JustMoney.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "JustMoney.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Login = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        BirthDay = c.DateTime(nullable: false),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        Country = c.Int(nullable: false),
                        Mobile = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("JustMoney.PrepaidCards", "UserId", c => c.Int());
            AddColumn("JustMoney.PrepaidCards", "Name", c => c.String(nullable: false));
			RenameColumn("JustMoney.PrepaidCards", "CardHolderId", "InternalToken");
            DropColumn("JustMoney.PrepaidCards", "LastName");
            DropColumn("JustMoney.PrepaidCards", "FirstName");
            DropColumn("JustMoney.PrepaidCards", "State");
            CreateIndex("JustMoney.PrepaidCards", "UserId");
            AddForeignKey("JustMoney.PrepaidCards", "UserId", "JustMoney.Users", "Id");
        }
    }
}
