namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceUid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entrances", "Uid", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entrances", "Uid");
        }
    }
}
