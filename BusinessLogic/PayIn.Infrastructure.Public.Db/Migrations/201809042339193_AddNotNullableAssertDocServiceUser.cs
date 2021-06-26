namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotNullableAssertDocServiceUser : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.ServiceUsers SET AssertDoc = '' WHERE AssertDoc is null");
            AlterColumn("dbo.ServiceUsers", "AssertDoc", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceUsers", "AssertDoc", c => c.String());
        }
    }
}
