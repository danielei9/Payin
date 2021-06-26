namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToLogin : DbMigration
    {
        public override void Up()
		{
			//Sql("ALTER TABLE[dbo].[ServiceSuppliers] DROP CONSTRAINT[DF__ServiceSu__Login__59FA5E80]");
			Sql("ALTER TABLE[dbo].[ServiceSuppliers] DROP CONSTRAINT[DF__ServiceSu__Login__6D0D32F4]");
			
			AlterColumn("dbo.ServiceUsers", "Login", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ServiceSuppliers", "Login", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.PaymentUsers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PaymentUsers", "Login", c => c.String(nullable: false, maxLength: 200));
			
			Sql("ALTER TABLE[dbo].[ServiceSuppliers] ADD DEFAULT('') FOR [Login]");
		}
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentUsers", "Login", c => c.String());
            AlterColumn("dbo.PaymentUsers", "Name", c => c.String());
            AlterColumn("dbo.ServiceSuppliers", "Login", c => c.String(nullable: false));
            AlterColumn("dbo.ServiceUsers", "Login", c => c.String(nullable: false));
        }
    }
}
