namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegexParaEntranceSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntranceSystemPaymentConcessions",
                c => new
                    {
                        EntranceSystem_Id = c.Int(nullable: false),
                        PaymentConcession_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntranceSystem_Id, t.PaymentConcession_Id })
                .ForeignKey("dbo.EntranceSystems", t => t.EntranceSystem_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcession_Id, cascadeDelete: true)
                .Index(t => t.EntranceSystem_Id)
                .Index(t => t.PaymentConcession_Id);

			Sql(
				"INSERT dbo.EntranceSystemPaymentConcessions (EntranceSystem_Id, PaymentConcession_Id) " +
				"SELECT ES.id, PC.id " +
				"FROM dbo.EntranceSystems ES, dbo.PaymentConcessions PC "
			);

			AddColumn("dbo.EntranceSystems", "RegEx", c => c.String(nullable: false, defaultValue: @"(.*)"));
			AddColumn("dbo.EntranceSystems", "RegExEventCode", c => c.Int());
			AddColumn("dbo.EntranceSystems", "RegExEntranceCode", c => c.Int(nullable: false, defaultValue: 1));
			AddColumn("dbo.EntranceSystems", "TemplateText", c => c.String(nullable: false, defaultValue: @"{2}"));
			AddColumn("dbo.EntranceSystems", "RegExText", c => c.String(nullable: false, defaultValue: @"(.*)"));
			AddColumn("dbo.EntranceSystems", "RegExEventCodeText", c => c.Int());
			AddColumn("dbo.EntranceSystems", "RegExEntranceCodeText", c => c.Int(nullable: false, defaultValue: 1));
			AlterColumn("dbo.EntranceSystems", "Template", c => c.String(nullable: false, defaultValue: @"{2}"));

			AddColumn("dbo.EntranceSystems", "IsDefault", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.EntranceSystems", "Name", c => c.String(nullable: false, defaultValue: "Sistema anónimo"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntranceSystemPaymentConcessions", "PaymentConcession_Id", "dbo.PaymentConcessions");
            DropForeignKey("dbo.EntranceSystemPaymentConcessions", "EntranceSystem_Id", "dbo.EntranceSystems");
            DropIndex("dbo.EntranceSystemPaymentConcessions", new[] { "PaymentConcession_Id" });
            DropIndex("dbo.EntranceSystemPaymentConcessions", new[] { "EntranceSystem_Id" });
            AlterColumn("dbo.EntranceSystems", "Template", c => c.String());
            AlterColumn("dbo.EntranceSystems", "Name", c => c.String());
            DropColumn("dbo.EntranceSystems", "RegExEntranceCodeText");
            DropColumn("dbo.EntranceSystems", "RegExEventCodeText");
            DropColumn("dbo.EntranceSystems", "RegExText");
            DropColumn("dbo.EntranceSystems", "TemplateText");
            DropColumn("dbo.EntranceSystems", "RegExEntranceCode");
            DropColumn("dbo.EntranceSystems", "RegExEventCode");
            DropColumn("dbo.EntranceSystems", "RegEx");
            DropColumn("dbo.EntranceSystems", "IsDefault");
            DropTable("dbo.EntranceSystemPaymentConcessions");
        }
    }
}
