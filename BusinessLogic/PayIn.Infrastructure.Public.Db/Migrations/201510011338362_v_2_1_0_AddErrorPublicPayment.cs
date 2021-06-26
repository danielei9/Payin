namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_2_1_0_AddErrorPublicPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "ErrorPublic", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "ErrorPublic");
        }
    }
}
