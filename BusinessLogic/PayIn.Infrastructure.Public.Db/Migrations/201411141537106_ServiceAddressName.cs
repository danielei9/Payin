namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceAddressName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceAddressNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProviderMap = c.Int(nullable: false),
                        ServiceAddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceAddresses", t => t.ServiceAddressId)
                .Index(t => t.ServiceAddressId);
            
            DropColumn("dbo.ServiceAddresses", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceAddresses", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.ServiceAddressNames", "ServiceAddressId", "dbo.ServiceAddresses");
            DropIndex("dbo.ServiceAddressNames", new[] { "ServiceAddressId" });
            DropTable("dbo.ServiceAddressNames");
        }
    }
}
