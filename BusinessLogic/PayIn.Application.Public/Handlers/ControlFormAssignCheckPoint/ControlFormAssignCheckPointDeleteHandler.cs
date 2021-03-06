using PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignCheckPointDeleteHandler :
		IServiceBaseHandler<ControlFormAssignCheckPointDeleteArguments>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignCheckPointDeleteHandler(
			IEntityRepository<ControlFormAssign> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormAssignCheckPointDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
