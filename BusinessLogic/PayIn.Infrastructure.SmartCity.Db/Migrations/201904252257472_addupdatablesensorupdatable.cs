namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addupdatablesensorupdatable : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Sensors", "Updatable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Sensors", "Updatable");
        }
    }
}
