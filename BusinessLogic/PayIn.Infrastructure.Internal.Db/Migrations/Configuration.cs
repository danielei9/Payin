namespace PayIn.Infrastructure.Internal.Db.Migrations
{
	using PayIn.Domain.Internal;
	using PayIn.Common;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InternalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "PayIn.Infrastructure.Internal.InternalContext";
        }

        protected override void Seed(InternalContext context)
        {
			// User
			CreateUser(context, new Migrations.Configuration.CreateUserDto
			{
				Login = "user@pay-in.es",
				Pin = "1234"
			});

			// Club
			CreateUser(context, new Migrations.Configuration.CreateUserDto
			{
				Login = "club@pay-in.es",
				Pin = "1234"
			});

			// ClubManager
			CreateUser(context, new Migrations.Configuration.CreateUserDto
			{
				Login = "clubmanager@pay-in.es",
				Pin = "1234"
			});

			var option = context.Options.Where(x => x.Name == "Versión").FirstOrDefault();
			if (option != null)
				context.Options.Remove(option);
			context.Options.AddOrUpdate(x => x.Name,
				new Option
				{
					Name = "ServerVersionName",
					Value = "4.1.11",
					ValueType = "string"
				});
		}

		#region CreateUser
		public class CreateUserDto
		{
			public string Login { get; set; }
			public string Pin { get; set; }
		}
		public void CreateUser(InternalContext context, CreateUserDto arguments)
		{
			context.User.AddOrUpdate(x => x.Login,
				new User { Login = arguments.Login, Pin = arguments.Pin.ToHash(), PinRetries = 0, State = UserState.Active }
			);
		}
		#endregion CreateUser
	}
}
