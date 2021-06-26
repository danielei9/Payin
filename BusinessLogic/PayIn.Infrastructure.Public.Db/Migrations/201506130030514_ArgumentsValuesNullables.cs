namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArgumentsValuesNullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean());
            AlterColumn("dbo.ControlFormValues", "ValueNumeric", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ControlFormValues", "ValueBool", c => c.Boolean());
            AlterColumn("dbo.ControlFormValues", "ValueDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlFormValues", "ValueDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ControlFormValues", "ValueBool", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ControlFormValues", "ValueNumeric", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean(nullable: false));
        }
    }
}
