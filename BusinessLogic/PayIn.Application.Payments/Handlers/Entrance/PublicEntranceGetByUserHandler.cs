using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class PublicEntranceGetByUserHandler :
		IQueryBaseHandler<PublicEntranceGetByUserArguments, MobileEntranceGetAllResult>
	{
		[Dependency] public MobileEntranceGetAllHandler MobileEntranceGetAllHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobileEntranceGetAllResult>> ExecuteAsync(PublicEntranceGetByUserArguments arguments)
		{
			DateTime now = DateTime.UtcNow;

			var entrances = await MobileEntranceGetAllHandler.ExecuteInternalAsync(now, arguments.Login, null, null, null);
			return new ResultBase<MobileEntranceGetAllResult>
			{
				Data = entrances
			};
		}
		#endregion ExecuteAsync
	}
}
