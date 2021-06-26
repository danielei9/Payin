using PayIn.Application.Dto.Arguments.ServicePrice;
using PayIn.Application.Dto.Results.ServicePrice;
using PayIn.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	//[XpAuthorize(ClientIds = "PayInWebApp")]
	[RoutePrefix("Api/ServicePrice")]
	public class ServicePriceController : ApiController
	{
		#region GET Api/ServicePrice/
		[Route("")]
		public async Task<ResultBase<ServicePriceGetAllResult>> Get(
			[FromUri] ServicePriceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServicePriceGetAllArguments, ServicePriceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET Api/ServicePrice/

		#region GET Api/ServicePrice/{id}
		[Route("{id}")]
		public async Task<ResultBase<ServicePriceGetResult>> Get(
			int id,
			[FromUri] ServicePriceGetArguments arguments,
			[Injection] IQueryBaseHandler<ServicePriceGetArguments, ServicePriceGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET Api/ServicePrice/{id}

		#region GET /Csv
		[HttpGet]
		public async Task<HttpResponseMessage> Csv(
			[FromUri] ServicePriceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ServicePriceGetAllArguments, ServicePriceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);

			var csv = new List<IEnumerable<string>> {
				new List<string> {
					"Id",
					"Time",
					"Price",
					"ZoneId",
					"ZoneName",
					"ConcessionId",
					"ConcessionName"
				}
			};
			csv.AddRange(
				result.Data
				.Select(x => new List<string> {
					x.Id.ToString(),
					x.Time.ToString(),
					x.Price.ToString(),
					x.ZoneId.ToString(),
					x.ZoneName,
					x.ConcessionId.ToString(),
					x.ConcessionName
				})
			);

			return Request.CreateResponse(HttpStatusCode.OK, csv, new CsvFormatter("Prices.csv"));
		}

		#endregion GET /Csv

		#region POST /
		public async Task<dynamic> Post(
			ServicePriceCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServicePriceCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { Id = item.Id };
		}
		#endregion POST /

		#region PUT /
		public async Task<dynamic> Put(
			int id,
			ServicePriceUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServicePriceUpdateArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /

		#region DELETE /
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ServicePriceDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ServicePriceDeleteArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /
	}
}
