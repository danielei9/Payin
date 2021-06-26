namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_2AddNumberInPromoExecution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoExecutions", "Number", c => c.Int(nullable: false,defaultValue : 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoExecutions", "Number");
        }
    }
}
