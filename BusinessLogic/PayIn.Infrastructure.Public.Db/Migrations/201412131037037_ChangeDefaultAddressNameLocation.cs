namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDefaultAddressNameLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceAddresses", "Name", c => c.String(nullable: false, defaultValue: "Default"));
						Sql("DELETE dbo.ServiceAddressNames where ProviderMap IS NULL");
            AlterColumn("dbo.ServiceAddressNames", "ProviderMap", c => c.Int(nullable: false));
        }
        public override void Down()
        {
            AlterColumn("dbo.ServiceAddressNames", "ProviderMap", c => c.Int());
            DropColumn("dbo.ServiceAddresses", "Name");
        }
    }
}
