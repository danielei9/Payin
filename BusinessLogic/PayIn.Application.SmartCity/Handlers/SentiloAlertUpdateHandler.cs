using Microsoft.Practices.Unity;
using PayIn.Application.Dto.SmartCity.Arguments;
using PayIn.Domain.SmartCity;
using System.Threading.Tasks;
using System;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.BusinessLogic.Common;

namespace PayIn.Application.SmartCity.Handlers
{
	public class SentiloAlertUpdateHandler : IServiceBaseHandler<SentiloAlertUpdateArguments>
	{
		[Dependency] public IEntityRepository<Alert> AlertRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SentiloAlertUpdateArguments arguments)
		{
			var now = DateTime.UtcNow;

			var item = await AlertRepository.GetAsync(arguments.Id);
			if (item == null)
				throw new ArgumentNullException(nameof(SentiloAlertUpdateArguments.Id));

			item.Alarms.Add(
				Alarm.Create(arguments.Message, now, SessionData.Login)
			);

			return item;
		}
		#endregion ExecuteAsync
	}
}
