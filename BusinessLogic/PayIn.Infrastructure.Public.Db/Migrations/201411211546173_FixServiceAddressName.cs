namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixServiceAddressName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ServiceAddressNames", name: "ServiceAddressId", newName: "AddressId");
            RenameIndex(table: "dbo.ServiceAddressNames", name: "IX_ServiceAddressId", newName: "IX_AddressId");
            AlterColumn("dbo.ServiceAddressNames", "ProviderMap", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceAddressNames", "ProviderMap", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.ServiceAddressNames", name: "IX_AddressId", newName: "IX_ServiceAddressId");
            RenameColumn(table: "dbo.ServiceAddressNames", name: "AddressId", newName: "ServiceAddressId");
        }
    }
}
