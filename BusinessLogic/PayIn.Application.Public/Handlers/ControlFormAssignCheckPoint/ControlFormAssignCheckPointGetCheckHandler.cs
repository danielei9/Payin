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
	public class ControlFormAssignCheckPointGetCheckHandler :
		IQueryBaseHandler<ControlFormAssignCheckPointGetCheckArguments, ControlFormAssignCheckPointGetCheckResult>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignCheckPointGetCheckHandler(
			IEntityRepository<ControlFormAssign> repository   
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;

		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignCheckPointGetCheckResult>> ExecuteAsync(ControlFormAssignCheckPointGetCheckArguments arguments)
		{
			var items = await Repository.GetAsync();
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Form.Name.Contains(arguments.Filter)
				));

			var CheckPointN = items
				.Where(x => x.CheckPointId == arguments.CheckId)
				.Select(x => x.CheckPoint.Name)
				.FirstOrDefault();

			var result = items
				.Where(x => x.CheckPointId == arguments.CheckId)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignCheckPointGetCheckResult
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.Form.Name,
					CheckPointName = x.CheckPoint.Name,
					Arguments = x.Form.Arguments
					.Select(y => new ControlFormAssignCheckPointGetCheckResult_Argument
					{
						Id = y.Id,
						Name = y.Name
					})
				});

			return new ControlFormAssignCheckPointGetCheckResultBase
			{
				CheckId = arguments.CheckId,
				CheckPointName = CheckPointN,
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
