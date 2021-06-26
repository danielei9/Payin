namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameAndVisitTimeStampsToRequestInBusRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.RequestStops", "VisitTimeStamp", c => c.DateTime());
            RenameColumn("Bus.Requests", "Note", "UserName");
        }
        
        public override void Down()
        {
            RenameColumn("Bus.Requests", "UserName", "Note");
            DropColumn("Bus.RequestStops", "VisitTimeStamp");
        }
    }
}
