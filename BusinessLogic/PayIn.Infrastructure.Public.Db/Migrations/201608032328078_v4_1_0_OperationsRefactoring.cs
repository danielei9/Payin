namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_OperationsRefactoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "ConfirmationDate", c => c.DateTime());
            AlterColumn("dbo.TransportOperations", "Uid", c => c.Long());
            DropColumn("dbo.TransportOperations", "ConfirmationRechargeDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportOperations", "ConfirmationRechargeDate", c => c.DateTime());
            AlterColumn("dbo.TransportOperations", "Uid", c => c.Int(nullable: false));
            DropColumn("dbo.TransportOperations", "ConfirmationDate");
        }
    }
}
