namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceConfigurationData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceConfigurationDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AwaitTimeAlert = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceConfigurationDatas");
        }
    }
}
