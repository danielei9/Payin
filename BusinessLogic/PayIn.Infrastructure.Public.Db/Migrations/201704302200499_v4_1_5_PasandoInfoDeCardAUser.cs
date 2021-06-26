namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_5_PasandoInfoDeCardAUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "OwnerUserId", c => c.Int());
            AddColumn("dbo.ServiceUsers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.ServiceUsers", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.ServiceUsers", "Photo", c => c.String(nullable: false));
            AddColumn("dbo.ServiceUsers", "VatNumber", c => c.String(nullable: false));
            CreateIndex("dbo.ServiceCards", "OwnerUserId");
            AddForeignKey("dbo.ServiceCards", "OwnerUserId", "dbo.ServiceUsers", "Id");

			Sql(
				"UPDATE [dbo].[ServiceUsers] " +
				"SET Name=C.Name, LastName=C.LastName, VatNumber=C.VatNumber, Photo=C.Photo " +
				"FROM " +
					"[dbo].[ServiceUsers] U inner join " +
					"[dbo].[ServiceCards] C on(U.cardid = C.id)"
			);
			Sql(
			   "UPDATE [dbo].[ServiceCards] " +
			   "SET OwnerUserId=U.id " +
			   "FROM " +
				   "[dbo].[ServiceCards] C inner join " +
				   "[dbo].[ServiceUsers] U on (U.cardid = C.id)"
		   );

			DropColumn("dbo.ServiceCards", "Name");
            DropColumn("dbo.ServiceCards", "LastName");
            DropColumn("dbo.ServiceCards", "Login");
            DropColumn("dbo.ServiceCards", "Photo");
            DropColumn("dbo.ServiceCards", "VatNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceCards", "VatNumber", c => c.String(nullable: false));
            AddColumn("dbo.ServiceCards", "Photo", c => c.String(nullable: false));
            AddColumn("dbo.ServiceCards", "Login", c => c.String(nullable: false));
            AddColumn("dbo.ServiceCards", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.ServiceCards", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.ServiceCards", "OwnerUserId", "dbo.ServiceUsers");
            DropIndex("dbo.ServiceCards", new[] { "OwnerUserId" });
			Sql(
				"UPDATE [dbo].[ServiceCards] " +
				"SET Name=U.Name, LastName=U.LastName, VatNumber=U.VatNumber, Photo=U.Photo, Login=U.Login " +
				"FROM " +
					"[dbo].[ServiceCards] C inner join " +
					"[dbo].[ServiceUsers] U on (c.OwnerUserId=U.id)"
			);

			DropColumn("dbo.ServiceUsers", "VatNumber");
            DropColumn("dbo.ServiceUsers", "Photo");
            DropColumn("dbo.ServiceUsers", "LastName");
            DropColumn("dbo.ServiceUsers", "Name");
            DropColumn("dbo.ServiceCards", "OwnerUserId");
        }
    }
}
