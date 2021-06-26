using PayIn.Application.Dto.Arguments.ControlFormArgument;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormArgumentDeleteHandler :
			IServiceBaseHandler<ControlFormArgumentDeleteArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ControlFormArgument> _Repository;

		#region Constructors
		public ControlFormArgumentDeleteHandler(
			IEntityRepository<PayIn.Domain.Public.ControlFormArgument> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			_Repository = repository;
		}
		#endregion Constructors

		#region ControlFormDelete
		async Task<dynamic> IServiceBaseHandler<ControlFormArgumentDeleteArguments>.ExecuteAsync(ControlFormArgumentDeleteArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);

			//await _Repository.DeleteAsync(item);

			item.State = Common.ControlFormArgumentState.Deleted;

			return null;
		}
		#endregion ControlFormDelete
	}
}
