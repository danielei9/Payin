using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Arguments.User;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class UserCreateNotificationHandler :
		IServiceBaseHandler<UserCreateNotificationArguments>
	{
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly IEntityRepository<Device> Repository;

		#region Constructors
		public UserCreateNotificationHandler(
			ServiceNotificationCreateHandler serviceNotificationCreate,
			IEntityRepository<Device> repository
		    
		)
		{
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			if (repository == null) throw new ArgumentNullException("repository");


			ServiceNotificationCreate = serviceNotificationCreate;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<UserCreateNotificationArguments>.ExecuteAsync(UserCreateNotificationArguments arguments)
		{

			if(arguments.AllUsers == true)
			{
				//var users = (await Repository.GetAsync("Platform"));
				var users = (await Repository.GetAsync());

				var usersList = users.ToList();

				foreach (var user in usersList)
				{
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.Personalized,
						message: arguments.Text,
						referenceId: null,
						referenceClass: "User",
						senderLogin: "info@pay-in.es",
						receiverLogin: user.Login
					));
				}
			}
			else
			{

				foreach (var user in arguments.Users)
				{					
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.Personalized,
					message: arguments.Text,
					referenceId: null,
					referenceClass: "User",
					senderLogin: "info@pay-in.es",
					receiverLogin: user.ToString()
				));

				}
			}
			
			return null;
		}
		#endregion ExecuteAsync
	}
}
