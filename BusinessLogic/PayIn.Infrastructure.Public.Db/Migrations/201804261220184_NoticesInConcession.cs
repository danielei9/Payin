namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticesInConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "PaymentConcessionId", c => c.Int(nullable: true));
            //Sql(
            //    "DELETE dbo.Notices " +
            //    "WHERE eventId is null"
            //);
            Sql(
                "UPDATE dbo.Notices " +
                "SET PaymentConcessionId = E.PaymentConcessionId " +
                "FROM Events E " +
                "WHERE dbo.Notices.eventId = E.id"
            );
            AlterColumn("dbo.Notices", "PaymentConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notices", "PaymentConcessionId");
            AddForeignKey("dbo.Notices", "PaymentConcessionId", "dbo.PaymentConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notices", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.Notices", new[] { "PaymentConcessionId" });
            DropColumn("dbo.Notices", "PaymentConcessionId");
        }
    }
}
