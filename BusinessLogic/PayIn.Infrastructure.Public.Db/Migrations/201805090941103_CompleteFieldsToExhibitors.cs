namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteFieldsToExhibitors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exhibitors", "Fax", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "PostalCode", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "City", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Province", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Url", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Pavilion", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Stand", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exhibitors", "Stand");
            DropColumn("dbo.Exhibitors", "Pavilion");
            DropColumn("dbo.Exhibitors", "Url");
            DropColumn("dbo.Exhibitors", "Country");
            DropColumn("dbo.Exhibitors", "Province");
            DropColumn("dbo.Exhibitors", "City");
            DropColumn("dbo.Exhibitors", "PostalCode");
            DropColumn("dbo.Exhibitors", "Fax");
        }
    }
}
