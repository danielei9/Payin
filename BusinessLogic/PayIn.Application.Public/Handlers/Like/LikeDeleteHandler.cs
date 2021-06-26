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
	public class LikeDeleteHandler :
		IServiceBaseHandler<LikeDeleteArguments>
	{
		//[Dependency] public IEntityRepository<LikeEvent> EventLikeRepository { get; set; }
		//[Dependency] public IEntityRepository<LikeExhibitor> ExhibitorLikeRepository { get; set; }
		//[Dependency] public IEntityRepository<LikeNotice> NoticeLikeRepository { get; set; }
		//[Dependency] public IEntityRepository<LikeActivity> ActivityLikeRepository { get; set; }

		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(LikeDeleteArguments arguments)
		{
			if (arguments.Id <= 0) throw new ArgumentNullException("Id");

			//var repository;
			switch (arguments.Type)
			{
				case LikeType.Notice:
					//repository = NoticeLikeRepository;
					break;

				case LikeType.Exhibitor:
					//repository = ExhibitorLikeRepository;
					break;

				case LikeType.Activity:
					//repository = ActivityLikeRepository;
					break;

				case LikeType.Event:
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
