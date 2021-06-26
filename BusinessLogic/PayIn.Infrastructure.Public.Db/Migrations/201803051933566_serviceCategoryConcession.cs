namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceCategoryConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCategories", "ServiceConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceCategories", "ServiceConcessionId");
            AddForeignKey("dbo.ServiceCategories", "ServiceConcessionId", "dbo.ServiceConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCategories", "ServiceConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ServiceCategories", new[] { "ServiceConcessionId" });
            DropColumn("dbo.ServiceCategories", "ServiceConcessionId");
        }
    }
}
