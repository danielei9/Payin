namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentsResponseCodeAndOrderId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "OrderId", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "ResponseCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "ResponseCode");
            DropColumn("dbo.Payments", "OrderId");
        }
    }
}
