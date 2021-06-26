using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class NotificationSetCreateHandler :
		IServiceBaseHandler<NotificationSetCreateArguments>
	{
		//[Dependency] public IEntityRepository<NotificationSet> notificationSetRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(NotificationSetCreateArguments arguments)
		{

			//await notificationSetRepository.AddAsync(new NotificationSet
			//{
			//	EventId = arguments.EventId,
			//	Message = arguments.Message
			//});
			
			// Llamar al proceso de envío de notificaciones

			return true;
		}
		#endregion ExecuteAsync
	}
}
