using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserCreateCardSelectGetHandler : 
		IQueryBaseHandler<ServiceUserCreateCardSelectGetArguments, ServiceUserCreateCardSelectGetResult>
	{
		private readonly ApiSystemCardGetSelectorHandler Handler;

		#region Constructors
		public ServiceUserCreateCardSelectGetHandler(
			ApiSystemCardGetSelectorHandler handler
		)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			Handler = handler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserCreateCardSelectGetResult>> ExecuteAsync(ServiceUserCreateCardSelectGetArguments arguments)
		{
			var result = await Handler.ExecuteAsync(new ApiSystemCardGetSelectorArguments(""));

			return new ServiceUserCreateCardSelectGetResultBase {
				SystemCardId = result.Data
			};
		}
		#endregion ExecuteAsync
	}
}
