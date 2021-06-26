namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountLineConcessionNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AccountLines", new[] { "ConcessionId" });
            AlterColumn("dbo.AccountLines", "ConcessionId", c => c.Int());
            CreateIndex("dbo.AccountLines", "ConcessionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AccountLines", new[] { "ConcessionId" });
            AlterColumn("dbo.AccountLines", "ConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.AccountLines", "ConcessionId");
        }
    }
}
