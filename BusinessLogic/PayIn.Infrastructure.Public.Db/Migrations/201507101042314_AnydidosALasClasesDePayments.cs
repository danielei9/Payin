namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnydidosALasClasesDePayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "UserLogin", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierName", c => c.String(nullable: false));
            DropColumn("dbo.Payments", "Name");
            DropColumn("dbo.Tickets", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Tickets", "SupplierName");
            DropColumn("dbo.Payments", "UserLogin");
            DropColumn("dbo.Payments", "UserName");
        }
    }
}
