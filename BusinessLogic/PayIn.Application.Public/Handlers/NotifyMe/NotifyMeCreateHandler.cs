using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class NotifyMeCreateHandler :
		IServiceBaseHandler<NotifyMeCreateArguments>
	{
		//[Dependency] public IEntityRepository<LikeEvent> EventNotifyMeRepository { get; set; }
		//[Dependency] public IEntityRepository<LikeActivity> ActivityNotifyMeRepository { get; set; }

		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NotifyMeCreateArguments arguments)
		{
			if (arguments.Id <= 0) throw new ArgumentNullException("Id");

			//var date = DateTime.Now.ToUTC();

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

			// Buscar el registro, y crearlo si no existe, añadiendo un registro para arguments.Id y el login de la sesión
			var login = SessionData.Login;
			var id = arguments.Id;


			return true;
		}
		#endregion ExecuteAsync
	}
}
