namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_TextError : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "ResponseText", c => c.String());
            AlterColumn("dbo.Payments", "ResponseCode", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "ResponseCode", c => c.Int(nullable: false));
            DropColumn("dbo.Payments", "ResponseText");
        }
    }
}
