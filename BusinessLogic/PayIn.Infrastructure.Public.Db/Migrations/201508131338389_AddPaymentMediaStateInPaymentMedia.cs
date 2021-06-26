namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentMediaStateInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMedias", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentMedias", "State");
        }
    }
}
