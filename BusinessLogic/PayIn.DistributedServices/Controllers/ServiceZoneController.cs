using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Application.Dto.Results.ServiceZone;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
				public class ServiceZoneController : ApiController
				{
								#region GET By Concession /GetByConcession
								[HttpGet]
								public async Task<ResultBase<ServiceZoneGetByConcessionResult>> GetByConcession(
												[FromUri] ServiceZoneGetByConcessionArguments command,
			[Injection] IQueryBaseHandler<ServiceZoneGetByConcessionArguments, ServiceZoneGetByConcessionResult> handler
								)
								{
												var result = await handler.ExecuteAsync(command);
												return result;
								}
								#endregion GET /

								#region GET Selector /Selector
								[HttpGet]
								public async Task<ResultBase<ServiceZoneGetSelectorResult>> Selector(
								[FromUri] ServiceZoneGetSelectorArguments command,
								[Injection] IQueryBaseHandler<ServiceZoneGetSelectorArguments, ServiceZoneGetSelectorResult> handler
								)
								{
												var result = await handler.ExecuteAsync(command);
												return result;
								}
								#endregion GET Selector /Selector

								#region POST /
								[HttpPost]
								public async Task<dynamic> Post(
												ServiceZoneCreateArguments command,
			[Injection] IServiceBaseHandler<ServiceZoneCreateArguments> handler
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
												ServiceZoneUpdateArguments command,
			[Injection] IServiceBaseHandler<ServiceZoneUpdateArguments> handler
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
												[FromUri] ServiceZoneDeleteArguments command,
			[Injection] IServiceBaseHandler<ServiceZoneDeleteArguments> handler
								)
								{
												var result = await handler.ExecuteAsync(command);
												return result;
								}
								#endregion DELETE /
				}
}
