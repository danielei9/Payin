using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntranceUpdateHandler :
		IServiceBaseHandler<ApiAccessControlEntranceUpdateArguments>
	{
		[Dependency] public IEntityRepository<AccessControlEntrance> Repository { get; set; }

		#region ExecuteAsync

		public async Task<dynamic> ExecuteAsync(ApiAccessControlEntranceUpdateArguments arguments)
		{
			var entrance = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			if (entrance == null)
				throw new ArgumentNullException("id");

            entrance.Name = arguments.Name;

			return entrance;
		}

		#endregion
	}
}
