namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceNumberPlate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceNumberPlates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberPlate = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        Color = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceNumberPlates");
        }
    }
}
