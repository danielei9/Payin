namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSerialNumberBlackList : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlackLists", "SerialNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlackLists", "SerialNumber", c => c.Long(nullable: false));
        }
    }
}
