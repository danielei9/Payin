using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntranceCreateHandler : IServiceBaseHandler<ApiAccessControlEntranceCreateArguments>
	{
		[Dependency] public IEntityRepository<AccessControlEntrance> Repository { get; set; }
		
		#region ExecuteAsync

		public async Task<dynamic> ExecuteAsync(ApiAccessControlEntranceCreateArguments arguments)
		{
			var access = new AccessControlEntrance()
			{
				Name = arguments.Name,
				AccessControlId = arguments.AccessControlId
			};

            await Repository.AddAsync(access);

			return access;
		}

		#endregion
	}
}
