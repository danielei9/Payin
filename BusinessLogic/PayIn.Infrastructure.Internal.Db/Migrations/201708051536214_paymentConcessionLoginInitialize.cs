namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentConcessionLoginInitialize : DbMigration
    {
        public override void Up()
        {
			Sql(
				"UPDATE internal.PaymentMedias " +
				"SET PaymentConcessionLogin = U.Login " +
				"FROM " +
					"internal.PaymentMedias PM INNER JOIN " +
					"internal.Users	U ON PM.UserId = U.Id "
			);
		}
        
        public override void Down()
        {
        }
    }
}
