namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncidenceConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceIncidences", "ConcessionId", c => c.Int(nullable: true));

            Sql(
                "UPDATE ServiceIncidences " +
                "SET ConcessionId=SC.id " +
                "FROM ServiceConcessions SC " +
                "WHERE SC.name='Vilamarxant' "
            );

            AlterColumn("dbo.ServiceIncidences", "ConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceIncidences", "ConcessionId");
            AddForeignKey("dbo.ServiceIncidences", "ConcessionId", "dbo.ServiceConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceIncidences", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ServiceIncidences", new[] { "ConcessionId" });
            DropColumn("dbo.ServiceIncidences", "ConcessionId");
        }
    }
}
