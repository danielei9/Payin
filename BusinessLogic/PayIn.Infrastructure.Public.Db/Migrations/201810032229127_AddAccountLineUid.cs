namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountLineUid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountLines", "Uid", c => c.Long());
            DropColumn("dbo.AccountLines", "Acount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountLines", "Acount", c => c.String());
            DropColumn("dbo.AccountLines", "Uid");
        }
    }
}
