using PayIn.Application.Dto.Tsm.Arguments.Card;
using Xp.Application.Dto;
using System.Threading.Tasks;

namespace PayIn.Application.Tsm.Arguments.Card
{
	public class CheckHandler : IServiceBaseHandler<CheckArguments>
	{
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CheckArguments arguments)
		{

			return null;
		}
		#endregion ExecuteAsync
	}
}
