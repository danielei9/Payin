using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Application.Dto.Results.ServiceTimeTable;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Formatters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
				public class ServiceTimeTableController : ApiController
				{
								#region GET /
								[HttpGet]
								public async Task<ResultBase<ServiceTimeTableGetAllResult>> Get(
									[FromUri] ServiceTimeTableGetAllArguments arguments,
									[Injection] IQueryBaseHandler<ServiceTimeTableGetAllArguments, ServiceTimeTableGetAllResult> handler
								)
								{
												var result = await handler.ExecuteAsync(arguments);
												return result;
								}
								#endregion GET /

								#region Get By Id /
								[HttpGet]
								public async Task<ResultBase<ServiceTimeTableGetByIdResult>> Get(
								int id,
								[FromUri] ServiceTimeTableGetByIdArguments query,
								[Injection] IQueryBaseHandler<ServiceTimeTableGetByIdArguments, ServiceTimeTableGetByIdResult> handler
								)
								{
												query.Id = id;
												return await handler.ExecuteAsync(query);
								}
								#endregion

								#region GET /Csv
								[HttpGet]
								public async Task<HttpResponseMessage> Csv(
									[FromUri] ServiceTimeTableGetAllArguments arguments,
									[Injection] IQueryBaseHandler<ServiceTimeTableGetAllArguments, ServiceTimeTableGetAllResult> handler
								)
								{
												var result = await handler.ExecuteAsync(arguments);

												var csv = new List<IEnumerable<string>> {
				new List<string> {
					"Id",
					"FromWeekday",
					"FromWeekdayLabel",
					"UntilWeekday",
					"UntilWeekdayLabel",
					"FromHour",
					"UntilHour",
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
					((int) x.FromWeekday).ToString(),
					x.FromWeekdayLabel,
					((int) x.UntilWeekday).ToString(),
					x.UntilWeekdayLabel,
					x.FromHour.ToString(),
					x.UntilHour.ToString(),
					x.ZoneId.ToString(),
					x.ZoneName,
					x.ConcessionId.ToString(),
					x.ConcessionName
				})
												);

												return Request.CreateResponse(HttpStatusCode.OK, csv, new CsvFormatter("TimeTable.csv"));
								}

								#endregion GET /Csv

								#region POST /
								[HttpPost]
								public async Task<dynamic> Post(
										ServiceTimeTableCreateArguments command,
										[Injection] IServiceBaseHandler<ServiceTimeTableCreateArguments> handler
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
										ServiceTimeTableUpdateArguments command,
										[Injection] IServiceBaseHandler<ServiceTimeTableUpdateArguments> handler
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
										[FromUri] ServiceTimeTableDeleteArguments command,
										[Injection] IServiceBaseHandler<ServiceTimeTableDeleteArguments> handler
								)
								{
												command.Id = id;
												var result = await handler.ExecuteAsync(command);
												return result;
								}
								#endregion DELETE /
				}
}
