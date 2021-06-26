namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToLoginInPaymentWorkers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentWorkers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PaymentWorkers", "Login", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentWorkers", "Login", c => c.String());
            AlterColumn("dbo.PaymentWorkers", "Name", c => c.String());
        }
    }
}
