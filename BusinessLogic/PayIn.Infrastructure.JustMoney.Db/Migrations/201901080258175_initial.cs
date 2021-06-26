namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "JustMoney.Options",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        ValueType = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "JustMoney.PaymentMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        InternalToken = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("JustMoney.PaymentMedias");
            DropTable("JustMoney.Options");
        }
    }
}
