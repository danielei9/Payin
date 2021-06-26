namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountLineUidFormat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountLines", "UidFormat", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountLines", "UidFormat");
        }
    }
}
