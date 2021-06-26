namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCardType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "Type", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCards", "Type");
        }
    }
}
