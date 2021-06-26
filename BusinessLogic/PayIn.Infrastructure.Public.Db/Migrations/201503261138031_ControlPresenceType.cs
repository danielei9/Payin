namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ControlPresenceType : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.ControlPresences", "PresenceType", c => c.Int());
			Sql("UPDATE dbo.ControlPresences SET PresenceType = TagType");
			DropColumn("dbo.ControlPresences", "TagType");
		}

		public override void Down()
		{
			AddColumn("dbo.ControlPresences", "TagType", c => c.Int());
			Sql("UPDATE dbo.ControlPresences SET TagType = PresenceType");
			DropColumn("dbo.ControlPresences", "PresenceType");
		}
	}
}
