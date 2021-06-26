using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceDocumentGetCreateHandler : IQueryBaseHandler<ServiceDocumentGetCreateArguments, ServiceDocumentGetCreateResult>
	{
		[Dependency] public ApiSystemCardGetSelectorHandler ApiSystemCardGetSelectorHandler { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<ServiceDocumentGetCreateResult>> ExecuteAsync(ServiceDocumentGetCreateArguments arguments)
		{
			var items = (await ApiSystemCardGetSelectorHandler.ExecuteAsync(new ApiSystemCardGetSelectorArguments(""))).Data
				.ToList();
			var @default = items
				.FirstOrDefault();

			return new ServiceDocumentGetCreateResultBase {
				Data = new []
				{
					new ServiceDocumentGetCreateResult
					{
						SystemCardId = @default?.Id,
						SystemCardName = @default?.Value ?? ""
					}
				},
				SystemCardId = items
			};
		}
		#endregion ExecuteAsync
	}
}

