using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.BusinessLogic.Common;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Bus.Handlers
{
	public class BusMobileRequestCreateHandler : IServiceBaseHandler<BusMobileRequestCreateArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public BusApiRequestCreateHandler BusApiRequestCreateHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BusMobileRequestCreateArguments arguments)
		{
			var newArguments = new BusApiRequestCreateArguments(SessionData.Name, arguments.FromId, arguments.ToId);
			newArguments.LineId = arguments.LineId;
			return await BusApiRequestCreateHandler.ExecuteAsync(newArguments);
		}
		#endregion ExecuteAsync
	}
}
