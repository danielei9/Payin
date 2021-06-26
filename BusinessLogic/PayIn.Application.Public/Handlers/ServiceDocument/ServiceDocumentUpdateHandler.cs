using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceDocumentUpdateHandler : IServiceBaseHandler<ServiceDocumentUpdateArguments>
	{
		[Dependency] public IEntityRepository<ServiceDocument> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceDocumentUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			item.Since = arguments.Since;
			item.Until = arguments.Until;
			item.Name = arguments.Name;

			return item;
		}
		#endregion ExecuteAsync
	}
}
