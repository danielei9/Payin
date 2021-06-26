namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToComponent : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Components", "Code", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Components", "Code");
        }
    }
}
