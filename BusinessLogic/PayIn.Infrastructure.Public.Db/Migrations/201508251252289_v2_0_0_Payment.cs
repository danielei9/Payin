namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_Payment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "RefundFromId", c => c.Int());
            CreateIndex("dbo.Payments", "RefundFromId");
            AddForeignKey("dbo.Payments", "RefundFromId", "dbo.Payments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "RefundFromId", "dbo.Payments");
            DropIndex("dbo.Payments", new[] { "RefundFromId" });
            DropColumn("dbo.Payments", "RefundFromId");
        }
    }
}
