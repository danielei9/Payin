namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormValueIsRequiredNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean());
        }
    }
}
