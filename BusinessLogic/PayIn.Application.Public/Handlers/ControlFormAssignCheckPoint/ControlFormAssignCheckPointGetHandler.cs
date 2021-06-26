using PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint;
using PayIn.Application.Dto.Results.ControlFormAssignCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignCheckPointGetHandler :
		IQueryBaseHandler<ControlFormAssignCheckPointGetArguments, ControlFormAssignCheckPointGetResult>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignCheckPointGetHandler(
			IEntityRepository<ControlFormAssign> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignCheckPointGetResult>> ExecuteAsync(ControlFormAssignCheckPointGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignCheckPointGetResult
				{
					Id = x.Id,
					Arguments = x.Form.Arguments
					.Select(y => new ControlFormAssignCheckPointGetResult_Argument
					{
						Id = y.Id,
						Name = y.Name,
						Observations = y.Observations,
						MinOptions = y.MinOptions,
						MaxOptions = y.MaxOptions,
						Target = y.Target,
						Type = y.Type
					})
				});

			return new ResultBase<ControlFormAssignCheckPointGetResult> { Data = items };
		}
		#endregion ExecuteAsync
	}
}
