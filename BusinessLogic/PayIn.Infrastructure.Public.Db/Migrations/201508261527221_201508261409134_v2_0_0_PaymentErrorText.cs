namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201508261409134_v2_0_0_PaymentErrorText : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "ErrorCode", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "ErrorText", c => c.String(nullable: false));
						Sql("UPDATE dbo.Payments SET AuthorizationCode = '' WHERE AuthorizationCode IS NULL");
            AlterColumn("dbo.Payments", "AuthorizationCode", c => c.String(nullable: false));
            DropColumn("dbo.Payments", "ResponseCode");
            DropColumn("dbo.Payments", "ResponseText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "ResponseText", c => c.String());
            AddColumn("dbo.Payments", "ResponseCode", c => c.Int());
            AlterColumn("dbo.Payments", "AuthorizationCode", c => c.String());
            DropColumn("dbo.Payments", "ErrorText");
            DropColumn("dbo.Payments", "ErrorCode");
        }
    }
}
