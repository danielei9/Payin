using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Domain.Bus;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiRequestDeleteHandler : IServiceBaseHandler<BusApiRequestDeleteArguments>
	{
		[Dependency] public IEntityRepository<Request> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BusApiRequestDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "From", "To");
			if (item == null)
				throw new ArgumentNullException(nameof(arguments.Id));

			item.From.State = Domain.Bus.Enums.RequestNodeState.Deleted;
			item.To.State = Domain.Bus.Enums.RequestNodeState.Deleted;

			return item;
		}
		#endregion ExecuteAsync
	}
}
