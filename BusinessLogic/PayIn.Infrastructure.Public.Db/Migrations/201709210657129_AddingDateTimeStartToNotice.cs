namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateTimeStartToNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "Start", c => c.DateTime(nullable: false, defaultValueSql:"2017/1/1"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "Start");
        }
    }
}
