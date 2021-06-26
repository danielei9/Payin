namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnyadirHorarioAlBus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Bus.TimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        LineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Bus.Lines", t => t.LineId)
                .Index(t => t.LineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Bus.TimeTables", "LineId", "Bus.Lines");
            DropIndex("Bus.TimeTables", new[] { "LineId" });
            DropTable("Bus.TimeTables");
        }
    }
}
