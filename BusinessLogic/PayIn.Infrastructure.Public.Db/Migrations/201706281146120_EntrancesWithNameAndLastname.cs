namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntrancesWithNameAndLastname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entrances", "LastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
			Sql("UPDATE dbo.Entrances SET UserName = TRIM(UserName + ' ' + LastName)");
            DropColumn("dbo.Entrances", "LastName");
        }
    }
}
