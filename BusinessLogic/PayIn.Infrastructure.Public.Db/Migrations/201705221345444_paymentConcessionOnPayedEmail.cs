namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentConcessionOnPayedEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "OnPayedEmail", c => c.String(nullable: false));
			Sql("UPDATE dbo.PaymentConcessions SET OnPayedEmail=''");
			AlterColumn("dbo.PaymentConcessions", "OnPayedEmail", c => c.String(nullable: true));
		}
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "OnPayedEmail");
        }
    }
}
