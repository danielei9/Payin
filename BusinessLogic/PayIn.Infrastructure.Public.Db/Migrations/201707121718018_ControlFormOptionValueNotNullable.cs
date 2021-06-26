namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormOptionValueNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlFormOptions", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlFormOptions", "Value", c => c.Int());
        }
    }
}
