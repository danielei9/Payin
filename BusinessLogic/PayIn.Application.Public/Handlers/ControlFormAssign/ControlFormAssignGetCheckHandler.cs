using PayIn.Application.Dto.Arguments.ControlFormAssign;
using PayIn.Application.Dto.Results.ControlFormAssign;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignGetCheckHandler :
		IQueryBaseHandler<ControlFormAssignGetCheckArguments, ControlFormAssignGetCheckResult>
	{
		private readonly IEntityRepository<ControlPlanningCheck> RepositoryCheck;
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignGetCheckHandler(
			IEntityRepository<ControlPlanningCheck> repositoryCheck,
			IEntityRepository<ControlFormAssign> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryCheck == null) throw new ArgumentNullException("repositoryCheck");

			Repository = repository;
			RepositoryCheck = repositoryCheck;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignGetCheckResult>> ExecuteAsync(ControlFormAssignGetCheckArguments arguments)
		{
			var item = (await RepositoryCheck.GetAsync())
				.Where(x => x.Id == arguments.CheckId)
				.Select(x => new {
					Date = x.Date,
					WorkerId = x.Planning.WorkerId,
					WorkerName = x.Planning.Worker.Name
				})
				.FirstOrDefault();

			var items = await Repository.GetAsync();
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Form.Name.Contains(arguments.Filter)
				));
			var result = items
				.Where(x => x.CheckId == arguments.CheckId)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignGetCheckResult
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.Form.Name,
					Values = x.Values
						.Select(y => new ControlFormAssignGetCheckResult_Value
						{
							Id = y.Id,
							Name = y.Argument.Name,
							Type = y.Argument.Type,
							ValueBool = y.ValueBool,
							ValueDateTime = y.ValueDateTime,
							ValueNumeric = y.ValueNumeric,
							ValueString = y.ValueString
						})
				});

			return new ControlFormAssignGetCheckResultBase
			{ 
				CheckId = arguments.CheckId,
				CheckDateDT = item.Date,
				WorkerId = item.WorkerId,
				WorkerName = item.WorkerName,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
