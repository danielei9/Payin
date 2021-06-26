namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormArgumentRequiredNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlFormArguments", "Required", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlFormArguments", "Required", c => c.Boolean());
        }
    }
}
