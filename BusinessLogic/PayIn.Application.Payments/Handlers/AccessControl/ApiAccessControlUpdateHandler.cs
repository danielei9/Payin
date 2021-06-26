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
    public class ApiAccessControlUpdateHandler :
		IServiceBaseHandler<ApiAccessControlUpdateArguments>
	{
		[Dependency] public IEntityRepository<AccessControl> Repository { get; set; }

		#region ExecuteAsync

		public async Task<dynamic> ExecuteAsync(ApiAccessControlUpdateArguments arguments)
		{
			var enclosure = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			if (enclosure == null)
				throw new ArgumentNullException("id");

            enclosure.Name = arguments.Name;
			enclosure.Schedule = arguments.Schedule;
			enclosure.MaxCapacity = arguments.MaxCapacity;
			enclosure.MapUrl = arguments.Map;

			return enclosure;
		}

		#endregion
	}
}
