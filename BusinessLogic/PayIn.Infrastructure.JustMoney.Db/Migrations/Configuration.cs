namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
	using PayIn.Domain.JustMoney;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JustMoneyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JustMoneyContext context)
        {
			//context.Option.AddOrUpdate(p => p.Id,
			//	new Option { Id = 1, Name = "LastMessageId", ValueType = "int", Value = "1" }
			//);
        }
    }
}
