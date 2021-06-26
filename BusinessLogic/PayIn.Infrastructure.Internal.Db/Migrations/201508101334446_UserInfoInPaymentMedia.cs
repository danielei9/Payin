namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInfoInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            DropIndex("internal.PaymentMedias", new[] { "User_Id" });
            RenameColumn(table: "internal.PaymentMedias", name: "User_Id", newName: "UserId");
            AlterColumn("internal.PaymentMedias", "ExpirationMonth", c => c.Int());
            AlterColumn("internal.PaymentMedias", "ExpirationYear", c => c.Int());
            AlterColumn("internal.PaymentMedias", "UserId", c => c.Int(nullable: false));
            CreateIndex("internal.PaymentMedias", "UserId");
            DropColumn("internal.PaymentMedias", "Login");
        }
        
        public override void Down()
        {
            AddColumn("internal.PaymentMedias", "Login", c => c.String(nullable: false));
            DropIndex("internal.PaymentMedias", new[] { "UserId" });
            AlterColumn("internal.PaymentMedias", "UserId", c => c.Int());
            AlterColumn("internal.PaymentMedias", "ExpirationYear", c => c.Int(nullable: false));
            AlterColumn("internal.PaymentMedias", "ExpirationMonth", c => c.Int(nullable: false));
            RenameColumn(table: "internal.PaymentMedias", name: "UserId", newName: "User_Id");
            CreateIndex("internal.PaymentMedias", "User_Id");
        }
    }
}
