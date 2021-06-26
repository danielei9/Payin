namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentmediaLogin : DbMigration
    {
        public override void Up()
        {
            RenameColumn("internal.PaymentMedias", "PaymentConcessionLogin", "Login");
		}
        
        public override void Down()
		{
			RenameColumn("internal.PaymentMedias", "Login", "PaymentConcessionLogin");
		}
    }
}
