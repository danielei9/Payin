using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileEntranceGetByEventHandler :
		IQueryBaseHandler<MobileEntranceGetByEventArguments, MobilePaymentMediaGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public MobileEntranceGetAllHandler MobileEntranceGetAllHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> ExecuteAsync(MobileEntranceGetByEventArguments arguments)
		{
			var now = DateTime.UtcNow;

			var entrances = (await MobileEntranceGetAllHandler.ExecuteInternalAsync(now, SessionData.Login, null, null, arguments.EventId));

			return new MobilePaymentMediaGetAllResultBase
			{
				Entrances = entrances
			};
		}
		#endregion ExecuteAsync
	}
}
