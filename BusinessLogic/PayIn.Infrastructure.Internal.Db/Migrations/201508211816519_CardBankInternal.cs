namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CardBankInternal : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.PaymentMedias", "Reference", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.PaymentMedias", "Reference");
        }
    }
}
