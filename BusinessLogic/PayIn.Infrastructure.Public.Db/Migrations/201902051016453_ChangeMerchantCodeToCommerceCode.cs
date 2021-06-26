namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMerchantCodeToCommerceCode : DbMigration
    {
        public override void Up()
        {
			RenameColumn("dbo.Payments", "MerchantCode", "CommerceCode");
        }
        
        public override void Down()
		{
			RenameColumn("dbo.Payments", "CommerceCode", "MerchantCode");
        }
    }
}
