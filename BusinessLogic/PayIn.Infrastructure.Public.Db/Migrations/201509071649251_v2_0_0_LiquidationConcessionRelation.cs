namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class v2_0_0_LiquidationConcessionRelation : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Liquidations", "ConcessionId", c => c.Int(nullable: true));
			Sql(
				"UPDATE L " +
				"SET ConcessionId = T.ConcessionId " + // Comprobar que pasa porque es multivaluado
				"FROM " +
					"dbo.Liquidations L INNER JOIN " +
					"dbo.Payments P ON P.LiquidationId = L.Id INNER JOIN " +
					"dbo.Tickets T ON P.TicketId = T.Id "
			);
			AlterColumn("dbo.Liquidations", "ConcessionId", c => c.Int(nullable: false));
			CreateIndex("dbo.Liquidations", "ConcessionId");
			AddForeignKey("dbo.Liquidations", "ConcessionId", "dbo.PaymentConcessions", "Id");
		}

		public override void Down()
		{
			DropForeignKey("dbo.Liquidations", "ConcessionId", "dbo.PaymentConcessions");
			DropIndex("dbo.Liquidations", new[] { "ConcessionId" });
			DropColumn("dbo.Liquidations", "ConcessionId");
		}
	}
}
