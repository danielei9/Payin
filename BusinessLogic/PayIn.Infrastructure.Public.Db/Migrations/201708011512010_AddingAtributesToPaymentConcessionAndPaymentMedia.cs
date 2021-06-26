namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAtributesToPaymentConcessionAndPaymentMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "Key", c => c.String(nullable: false, defaultValue:""));
            AddColumn("dbo.PaymentConcessions", "KeyType", c => c.Int());
            AddColumn("dbo.PaymentMedias", "UserName", c => c.String(nullable: false, defaultValue:"UserName"));
            AddColumn("dbo.PaymentMedias", "UserLastName", c => c.String(nullable: false, defaultValue:"UserLastName"));
            AddColumn("dbo.PaymentMedias", "UserBirthday", c => c.DateTime());
            AddColumn("dbo.PaymentMedias", "UserTaxNumber", c => c.String(nullable: false, defaultValue:""));
            AddColumn("dbo.PaymentMedias", "UserAddress", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.PaymentMedias", "UserPhone", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.PaymentMedias", "UserEmail", c => c.String(nullable: false, defaultValue:"UserEmail"));
            AddColumn("dbo.PaymentMedias", "Default", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.PaymentMedias", "PaymentConcessionId", c => c.Int());
            CreateIndex("dbo.PaymentMedias", "PaymentConcessionId");
            AddForeignKey("dbo.PaymentMedias", "PaymentConcessionId", "dbo.PaymentConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentMedias", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.PaymentMedias", new[] { "PaymentConcessionId" });
            DropColumn("dbo.PaymentMedias", "PaymentConcessionId");
            DropColumn("dbo.PaymentMedias", "Default");
            DropColumn("dbo.PaymentMedias", "UserEmail");
            DropColumn("dbo.PaymentMedias", "UserPhone");
            DropColumn("dbo.PaymentMedias", "UserAddress");
            DropColumn("dbo.PaymentMedias", "UserTaxNumber");
            DropColumn("dbo.PaymentMedias", "UserBirthday");
            DropColumn("dbo.PaymentMedias", "UserLastName");
            DropColumn("dbo.PaymentMedias", "UserName");
            DropColumn("dbo.PaymentConcessions", "KeyType");
            DropColumn("dbo.PaymentConcessions", "Key");
        }
    }
}
