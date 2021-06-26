namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressNameDeleteOnCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceAddressNames", "AddressId", "dbo.ServiceAddresses");
            AddForeignKey("dbo.ServiceAddressNames", "AddressId", "dbo.ServiceAddresses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceAddressNames", "AddressId", "dbo.ServiceAddresses");
            AddForeignKey("dbo.ServiceAddressNames", "AddressId", "dbo.ServiceAddresses", "Id");
        }
    }
}
