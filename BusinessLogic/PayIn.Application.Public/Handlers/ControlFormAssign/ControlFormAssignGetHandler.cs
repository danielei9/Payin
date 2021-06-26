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
	public class ControlFormAssignGetHandler :
		IQueryBaseHandler<ControlFormAssignGetArguments, ControlFormAssignGetResult>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignGetHandler(
			IEntityRepository<ControlFormAssign> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignGetResult>> ExecuteAsync(ControlFormAssignGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == 11)
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new ControlFormAssignGetResult
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.Form.Name,
					PresencesCount = x.Check.ControlPresences.Count(),
					Values = x.Values
						.Select(y => new ControlFormAssignGetResult_Value
						{
							Id = y.Id,
							Name = y.Argument.Name,
							Observations = y.Observations,
							Target = y.Target,
							Type = y.Argument.Type,
							ValueBool = y.ValueBool,
							ValueDateTime = y.ValueDateTime,
							ValueNumeric = y.ValueNumeric,
							ValueString = y.ValueString
						})
				});

			return new ResultBase<ControlFormAssignGetResult> { Data = items };
		}
		#endregion ExecuteAsync
	}
}
