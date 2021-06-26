namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentMediaStateInPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.PaymentMedias", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.PaymentMedias", "State");
        }
    }
}
