namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_4_ServiceCardVatNumberNotAllowNull : DbMigration
    {
        public override void Up()
        {
			Sql("UPDATE dbo.ServiceCards SET VatNumber='' WHERE VatNumber IS NULL");
            AlterColumn("dbo.ServiceCards", "VatNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceCards", "VatNumber", c => c.String());
        }
    }
}
