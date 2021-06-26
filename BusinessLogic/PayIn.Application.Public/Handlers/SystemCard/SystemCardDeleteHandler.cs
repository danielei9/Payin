using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCard;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class SystemCardDeleteHandler :
			IServiceBaseHandler<SystemCardDeleteArguments>
	{
		[Dependency] public IEntityRepository<SystemCard> Repository { get; set; }

		#region SystemCardDelete
		async Task<dynamic> IServiceBaseHandler<SystemCardDeleteArguments>.ExecuteAsync(SystemCardDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
		//	await Repository.DeleteAsync(item);

			return null;
		}
		#endregion SystemCardDelete
	}
}
