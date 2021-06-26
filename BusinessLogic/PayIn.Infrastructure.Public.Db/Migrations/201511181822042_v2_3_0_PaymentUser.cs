namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_PaymentUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        State = c.Int(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentUsers", "ConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.PaymentUsers", new[] { "ConcessionId" });
            DropTable("dbo.PaymentUsers");
        }
    }
}
