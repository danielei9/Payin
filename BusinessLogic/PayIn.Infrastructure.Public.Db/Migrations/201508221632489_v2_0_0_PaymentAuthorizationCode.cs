namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_PaymentAuthorizationCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "AuthorizationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "AuthorizationCode");
        }
    }
}
