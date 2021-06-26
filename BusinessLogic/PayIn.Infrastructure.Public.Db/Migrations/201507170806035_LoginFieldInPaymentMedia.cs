namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginFieldInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMedias", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentMedias", "Login");
        }
    }
}
