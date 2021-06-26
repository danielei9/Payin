namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExhibitorMoreFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exhibitors", "Code", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "Contact", c => c.String(nullable: false));
            AddColumn("dbo.Exhibitors", "ContactEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exhibitors", "ContactEmail");
            DropColumn("dbo.Exhibitors", "Contact");
            DropColumn("dbo.Exhibitors", "Code");
        }
    }
}
