namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionsDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlIncidents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Observations = c.String(),
                        Source = c.String(),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceOptions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        ValueType = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceOptions");
            DropTable("dbo.ControlIncidents");
        }
    }
}
