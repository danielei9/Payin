namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIntoControlFormValue : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ControlFormValues", "Name");
            DropColumn("dbo.ControlFormValues", "Type");
            DropColumn("dbo.ControlFormValues", "IsRequired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean());
			Sql(
				"UPDATE dbo.ControlFormValues " +
				"SET IsRequired = A.MinOptions " +
				"FROM " +
					"dbo.ControlFormValues V INNER JOIN " +
					"dbo.ControlFormArguments A ON V.argumentId = A.id "
			);
			AlterColumn("dbo.ControlFormValues", "IsRequired", c => c.Boolean(nullable: false));


			AddColumn("dbo.ControlFormValues", "Type", c => c.Int());
			Sql(
				"UPDATE dbo.ControlFormValues " +
				"SET Type = A.Type " +
				"FROM " +
					"dbo.ControlFormValues V INNER JOIN " +
					"dbo.ControlFormArguments A ON V.argumentId = A.id "
			);
			AlterColumn("dbo.ControlFormValues", "Type", c => c.Int(nullable: false));

			AddColumn("dbo.ControlFormValues", "Name", c => c.String());
			Sql(
				"UPDATE dbo.ControlFormValues " +
				"SET Name = A.Name " +
				"FROM " +
					"dbo.ControlFormValues V INNER JOIN " +
					"dbo.ControlFormArguments A ON V.argumentId = A.id "
			);
			AlterColumn("dbo.ControlFormValues", "Name", c => c.String(nullable: false));
		}
	}
}
