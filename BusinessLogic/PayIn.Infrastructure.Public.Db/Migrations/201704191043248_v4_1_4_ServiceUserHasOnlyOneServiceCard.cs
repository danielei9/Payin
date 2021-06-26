namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_4_ServiceUserHasOnlyOneServiceCard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceUserServiceCards", "ServiceUser_Id", "dbo.ServiceUsers");
            DropForeignKey("dbo.ServiceUserServiceCards", "ServiceCard_Id", "dbo.ServiceCards");
            DropIndex("dbo.ServiceUserServiceCards", new[] { "ServiceUser_Id" });
            DropIndex("dbo.ServiceUserServiceCards", new[] { "ServiceCard_Id" });
            AddColumn("dbo.ServiceUsers", "CardId", c => c.Int());
            CreateIndex("dbo.ServiceUsers", "CardId");
            AddForeignKey("dbo.ServiceUsers", "CardId", "dbo.ServiceCards", "Id");

			Sql(
				"UPDATE dbo.ServiceUsers " +
				"SET CardId = T.ServiceCard_Id " +
				"FROM dbo.ServiceUserServiceCards T " +
				"WHERE T.ServiceUser_Id = Id "
			);

			DropTable("dbo.ServiceUserServiceCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceUserServiceCards",
                c => new
                    {
                        ServiceUser_Id = c.Int(nullable: false),
                        ServiceCard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceUser_Id, t.ServiceCard_Id });
            
            DropForeignKey("dbo.ServiceUsers", "CardId", "dbo.ServiceCards");
            DropIndex("dbo.ServiceUsers", new[] { "CardId" });

			Sql(
				"INSERT dbo.ServiceUserServiceCards (ServiceUser_Id, ServiceCard_Id) " +
				"SELECT Id, CardId " +
				"FROM dbo.ServiceUsers " +
				"WHERE CardId IS NOT NULL "
			);

			DropColumn("dbo.ServiceUsers", "CardId");
            CreateIndex("dbo.ServiceUserServiceCards", "ServiceCard_Id");
            CreateIndex("dbo.ServiceUserServiceCards", "ServiceUser_Id");
            AddForeignKey("dbo.ServiceUserServiceCards", "ServiceCard_Id", "dbo.ServiceCards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceUserServiceCards", "ServiceUser_Id", "dbo.ServiceUsers", "Id", cascadeDelete: true);
        }
    }
}
