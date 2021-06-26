namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_PaymentMediaPublicExpirationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMedias", "ExpirationMonth", c => c.Int());
            AddColumn("dbo.PaymentMedias", "ExpirationYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentMedias", "ExpirationYear");
            DropColumn("dbo.PaymentMedias", "ExpirationMonth");
        }
    }
}
