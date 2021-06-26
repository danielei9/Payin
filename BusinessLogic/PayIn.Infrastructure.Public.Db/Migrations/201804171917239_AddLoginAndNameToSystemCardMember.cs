namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginAndNameToSystemCardMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCardMembers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.SystemCardMembers", "Login", c => c.String(nullable: false));

            Sql(
                "UPDATE dbo.SystemCardMembers " +
                "SET " +
                    "Name=SC.Name, " +
                    "Login=SS.Login " +
                "FROM " +
                    "dbo.ServiceConcessions SC INNER JOIN " +
                    "dbo.ServiceSuppliers SS ON SC.supplierId=SS.Id " +
                "WHERE SC.id=SystemCardMembers.concessionId "
            );

            DropForeignKey("dbo.SystemCardMembers", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.SystemCardMembers", new[] { "ConcessionId" });
            DropColumn("dbo.SystemCardMembers", "ConcessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SystemCardMembers", "ConcessionId", c => c.Int(nullable: true));

            Sql(
                "UPDATE dbo.SystemCardMembers " +
                "SET " +
                    "concessionId=SC.id " +
                "FROM " +
                    "dbo.ServiceConcessions SC INNER JOIN " +
                    "dbo.ServiceSupplier SS ON SC.supplierId=SS.Id" +
                " WHERE SS.Login=SystemCardMembers.Login "
            );

            AlterColumn("dbo.SystemCardMembers", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.SystemCardMembers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.SystemCardMembers", "Login", c => c.String(nullable: false));
            DropColumn("dbo.SystemCardMembers", "Login");
            DropColumn("dbo.SystemCardMembers", "Name");
            CreateIndex("dbo.SystemCardMembers", "ConcessionId");
            AddForeignKey("dbo.SystemCardMembers", "ConcessionId", "dbo.ServiceConcessions", "Id");
        }
    }
}
