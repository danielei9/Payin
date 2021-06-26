namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagsToSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceTags", "SupplierId", c => c.Int());
						Sql(
							"UPDATE ServiceTags " +
							"SET SupplierId = SC.SupplierId " +
							"FROM " +
							  "ServiceTags ST INNER JOIN " +
							  "ControlItems CI ON CI.Id = ST.ItemId INNER JOIN " +
								"ServiceConcessions SC ON SC.id = CI.concessionId "
						);
						AlterColumn("dbo.ServiceTags", "SupplierId", c => c.Int(nullable: false));
						CreateIndex("dbo.ServiceTags", "SupplierId");
            AddForeignKey("dbo.ServiceTags", "SupplierId", "dbo.ServiceSuppliers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceTags", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.ServiceTags", new[] { "SupplierId" });
            DropColumn("dbo.ServiceTags", "SupplierId");
        }
    }
}
