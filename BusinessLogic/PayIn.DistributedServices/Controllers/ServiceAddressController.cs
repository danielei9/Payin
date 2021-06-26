using PayIn.Application.Dto.Arguments.ServiceAddress;
using PayIn.Application.Dto.Results.ServiceAddress;
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
	public class ServiceAddressController : ApiController
	{
		#region GET /
		public async Task<ResultBase<ServiceAddressGetAllResult>> Get(
			[FromUri] ServiceAddressGetAllArguments command,
			[Injection] IQueryBaseHandler<ServiceAddressGetAllArguments, ServiceAddressGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion GET /

		#region GET /Csv
		[HttpGet]
		public async Task<HttpResponseMessage> Csv(
			[FromUri] ServiceAddressGetAllArguments command,
			[Injection] IQueryBaseHandler<ServiceAddressGetAllArguments, ServiceAddressGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(command);

			var csv = new List<IEnumerable<string>> {
				new List<string> {
					"Id",
					"Name",
					"Side",
					"SideLabel",
					"From",
					"Until",
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
					x.Name,
					x.Side.ToString(),
					x.SideLabel,
					x.From.ToString(),
					x.Until.ToString(),
					x.ZoneId.ToString(),
					x.ZoneName,
					x.ConcessionId.ToString(),
					x.ConcessionName
				})
			);

			return Request.CreateResponse(HttpStatusCode.OK, csv, new CsvFormatter("Addresses.csv"));
		}
		#endregion GET /Csv

		#region POST /
		[HttpPost]
		public async Task<dynamic> Post(
				ServiceAddressCreateArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion POST /

		#region PUT /
		[HttpPut]
		public async Task<dynamic> Put(
				int id,
				ServiceAddressUpdateArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressUpdateArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { Id = item.Id };
		}
		#endregion PUT /

		#region DELETE /
		[HttpDelete]
		public async Task<dynamic> Delete(
				int id,
				[FromUri] ServiceAddressDeleteArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /
	}
}
