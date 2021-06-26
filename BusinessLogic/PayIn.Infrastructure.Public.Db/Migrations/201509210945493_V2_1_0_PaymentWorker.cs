namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2_1_0_PaymentWorker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentWorkers",
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

			Sql(
				"INSERT INTO dbo.PaymentWorkers(Name,Login,ConcessionId,State) " +
				"SELECT SS.Name, SS.Login, PC.Id, 1 " +
				"FROM " +
					"dbo.PaymentConcessions PC INNER JOIN " +
					"dbo.ServiceConcessions SC ON SC.Id = PC.ConcessionId INNER JOIN " +
					"dbo.ServiceSuppliers SS ON SS.Id = SC.SupplierId "
			);

            AddColumn("dbo.Tickets", "PaymentWorkerId", c => c.Int(nullable: false));
			Sql(
				"UPDATE TI " +
				"SET TI.PaymentWorkerId = PW.Id " +
				"FROM " +
					"dbo.Tickets TI INNER JOIN " +
					"dbo.PaymentConcessions PC ON PC.Id = TI.ConcessionId INNER JOIN " +
					"dbo.PaymentWorkers PW ON PW.ConcessionId = PC.Id");
            CreateIndex("dbo.Tickets", "PaymentWorkerId");
            AddForeignKey("dbo.Tickets", "PaymentWorkerId", "dbo.PaymentWorkers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentWorkers", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.Tickets", "PaymentWorkerId", "dbo.PaymentWorkers");
            DropIndex("dbo.Tickets", new[] { "PaymentWorkerId" });
            DropIndex("dbo.PaymentWorkers", new[] { "ConcessionId" });
            DropColumn("dbo.Tickets", "PaymentWorkerId");
            DropTable("dbo.PaymentWorkers");
        }
    }
}
