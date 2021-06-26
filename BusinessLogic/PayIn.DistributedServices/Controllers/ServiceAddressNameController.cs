using PayIn.Application.Dto.Arguments.ServiceAddressName;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class ServiceAddressNameController : ApiController
	{
		#region POST /
		[HttpPost]
		public async Task<dynamic> Post(
				ServiceAddressNameCreateArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressNameCreateArguments> handler
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
				ServiceAddressNameUpdateArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressNameUpdateArguments> handler
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
				[FromUri] ServiceAddressNameDeleteArguments command,
				[Injection] IServiceBaseHandler<ServiceAddressNameDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(command);
			return result;
		}
		#endregion DELETE /
	}
}
