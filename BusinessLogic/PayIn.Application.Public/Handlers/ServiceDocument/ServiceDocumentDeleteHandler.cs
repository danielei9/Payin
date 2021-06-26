using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceDocumentDeleteHandler :
		IServiceBaseHandler<ServiceDocumentDeleteArguments>
	{
		[Dependency] public IEntityRepository<ServiceDocument> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceDocumentDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
