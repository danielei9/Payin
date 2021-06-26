namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeqAndUidFormatToPayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "UidFormat", c => c.Int());
            AddColumn("dbo.Payments", "Seq", c => c.Int());
            //Sql("ALTER TABLE[dbo].[Payments] DROP CONSTRAINT[DF__Payments__Uid__1F198FD4]"); // No se eliminaba automáticamente
            DropColumn("dbo.Payments", "Uid");
            AddColumn("dbo.Payments", "Uid", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "Uid", c => c.String(nullable: false));
            DropColumn("dbo.Payments", "Seq");
            DropColumn("dbo.Payments", "UidFormat");
        }
    }
}
