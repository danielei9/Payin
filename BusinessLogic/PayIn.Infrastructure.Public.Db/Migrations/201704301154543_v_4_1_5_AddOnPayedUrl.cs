namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_5_AddOnPayedUrl : DbMigration
    {
        public override void Up()
		{
			AddColumn("dbo.PaymentConcessions", "OnPayedUrl", c => c.String(nullable: true));
			Sql(
				"UPDATE dbo.PaymentConcessions " +
				"SET OnPayedUrl = ''"
			);
			AlterColumn("dbo.PaymentConcessions", "OnPayedUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "OnPayedUrl");
        }
    }
}
