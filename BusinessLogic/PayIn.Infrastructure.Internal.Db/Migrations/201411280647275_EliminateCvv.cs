namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminateCvv : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.PaymentGateways", newSchema: "internal");
            MoveTable(name: "dbo.PaymentMedias", newSchema: "internal");
            MoveTable(name: "dbo.Users", newSchema: "internal");
            DropIndex("internal.PaymentMedias", new[] { "UserId" });
            RenameColumn(table: "internal.PaymentMedias", name: "UserId", newName: "User_Id");
            AddColumn("internal.PaymentMedias", "Login", c => c.String(nullable: false));
            AlterColumn("internal.PaymentMedias", "User_Id", c => c.Int());
            CreateIndex("internal.PaymentMedias", "User_Id");
            DropColumn("internal.PaymentMedias", "Cvv");
        }
        
        public override void Down()
        {
            AddColumn("internal.PaymentMedias", "Cvv", c => c.String(nullable: false));
            DropIndex("internal.PaymentMedias", new[] { "User_Id" });
            AlterColumn("internal.PaymentMedias", "User_Id", c => c.Int(nullable: false));
            DropColumn("internal.PaymentMedias", "Login");
            RenameColumn(table: "internal.PaymentMedias", name: "User_Id", newName: "UserId");
            CreateIndex("internal.PaymentMedias", "UserId");
            MoveTable(name: "internal.Users", newSchema: "dbo");
            MoveTable(name: "internal.PaymentMedias", newSchema: "dbo");
            MoveTable(name: "internal.PaymentGateways", newSchema: "dbo");
        }
    }
}
