namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_5_CorregirTableProduct : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Products", "FamilyID", "FamilyId" );
            RenameColumn("dbo.Products", "state","State");
            DropColumn("dbo.Products", "Category");
            DropColumn("dbo.Products", "Stock");
            DropColumn("dbo.Products", "Vat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Vat", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Stock", c => c.Long(nullable: false));
            AddColumn("dbo.Products", "Category", c => c.String(nullable: false));
			RenameColumn("dbo.Products", "FamilyId", "FamilyID");
			RenameColumn("dbo.Products", "State", "state");
		}
    }
}
