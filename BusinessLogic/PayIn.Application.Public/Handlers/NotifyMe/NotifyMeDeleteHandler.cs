using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class NotifyMeDeleteHandler :
		IServiceBaseHandler<NotifyMeDeleteArguments>
	{
		//[Dependency] public IEntityRepository<NotifyMeActivity> ActivityNotifyMeRepository { get; set; }
		//[Dependency] public IEntityRepository<NotifyMeEvent> EventNotifyMeRepository { get; set; }

		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NotifyMeDeleteArguments arguments)
		{
			if (arguments.Id <= 0) throw new ArgumentNullException("Id");

			//var repository;
			switch (arguments.Type)
			{
				case NotifyMeType.Activity:
					//repository = ActivityLikeRepository;
					break;

				case NotifyMeType.Event:
					//repository = EventLikeRepository;
					break;

				// default:
			}

			// Borrar el registro para arguments.Id y el login de la sesión
			var login = SessionData.Login;
			var id = arguments.Id;


			return true;
		}
		#endregion ExecuteAsync
	}
}
