namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SendMailWhenAffiliate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCards", "AffiliationEmailBody", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCards", "AffiliationEmailBody");
        }
    }
}
