namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Sensors", "Code", c => c.String());
            AddColumn("SmartCity.Providers", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Providers", "Code");
            DropColumn("SmartCity.Sensors", "Code");
        }
    }
}
