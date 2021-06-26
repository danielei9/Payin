namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Photo_to_PhotoUrl_ProductFamily : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ProductFamilies", "PhotoUrl", c => c.String(nullable: false));
            //DropColumn("dbo.ProductFamilies", "Photo");

            RenameColumn("dbo.ProductFamilies", "Photo", "PhotoUrl");
        }

        public override void Down()
        {
            //AddColumn("dbo.ProductFamilies", "Photo", c => c.String(nullable: false));
            //DropColumn("dbo.ProductFamilies", "PhotoUrl");

            RenameColumn("dbo.ProductFamilies", "PhotoUrl", "Photo");
        }
    }
}
