using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Application.Dto.Results.ServiceCheckPoint;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ServiceCheckPoint")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator
	)]
	public class ServiceCheckPointController : ApiController
	{
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		[Authorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceCheckPointGetResult>> Get(
			[FromUri] ServiceCheckPointGetArguments command,
			[Injection] IQueryBaseHandler<ServiceCheckPointGetArguments, ServiceCheckPointGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Item/{itemId:int}
		[HttpGet]
		[Route("Item/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceCheckPointGetItemChecksResult>> GetItemChecks(
			[FromUri] ServiceCheckPointGetItemChecksArguments arguments,
			[Injection] IQueryBaseHandler<ServiceCheckPointGetItemChecksArguments, ServiceCheckPointGetItemChecksResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Item/{itemId:int}

		#region GET /Selector/{filter}
		[HttpGet]
		[Route("Selector/{filter?}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceCheckPointGetSelectorResult>> Selector(
			string filter,
			[FromUri] ServiceCheckPointGetSelectorArguments command,
			[Injection] IQueryBaseHandler<ServiceCheckPointGetSelectorArguments, ServiceCheckPointGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /Selector/{filter}

		#region GET /SelectorCheck/{filter}
		[HttpGet]
		[Route("SelectorCheck/{filter?}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<ResultBase<ServiceCheckPointGetSelectorCheckResult>> SelectorCheck(
			string filter,
			[FromUri] ServiceCheckPointGetSelectorCheckArguments command,
			[Injection] IQueryBaseHandler<ServiceCheckPointGetSelectorCheckArguments, ServiceCheckPointGetSelectorCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /SelectorCheck/{filter}

		//#region GET /Csv
		//[HttpGet]
		//[Route("Csv")]
		//public async Task<HttpResponseMessage> Csv(
		//	[FromUri] ServiceCheckPointGetAllArguments command,
		//	[Injection] IQueryBaseHandler<ServiceCheckPointGetAllArguments, ServiceCheckPointGetAllResult> handler
		//)
		//{
		//	var result = await handler.ExecuteAsync(command);

		//	var csv = new List<IEnumerable<string>> {
		//		new List<string> {
		//			"Id",
		//			"Reference",
		//			"TagType",
		//			"TagTypeLabel",
		//			"SupplierId",
		//			"SupplierName"
		//		}
		//	};
		//	csv.AddRange(
		//		result.Data
		//		.Select(x => new List<string> {
		//			x.Id.ToString(),
		//			x.Reference,
		//			x.TagType.ToString(),
		//			x.TagTypeLabel,
		//			x.SupplierId.ToString(),
		//			x.SupplierName
		//		})
		//	);

		//	return Request.CreateResponse(HttpStatusCode.OK, csv, new CsvFormatter("Addresses.csv"));
		//}
		//#endregion GET /Csv

		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<dynamic> Post(
				ServiceCheckPointCreateArguments command,
				[Injection] IServiceBaseHandler<ServiceCheckPointCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<dynamic> Put(
			ServiceCheckPointUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceCheckPointUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Operator)]
		public async Task<dynamic> Delete(
			[FromUri] ServiceCheckPointDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServiceCheckPointDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

	}
}