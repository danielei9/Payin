using PayIn.Application.Dto.Arguments.ControlFormAssign;
using PayIn.Application.Dto.Results.ControlFormAssign;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignGetIdsHandler :
		IQueryBaseHandler<ControlFormAssignGetIdsArguments, ControlFormAssignGetIdsResult>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;

		#region Constructors
		public ControlFormAssignGetIdsHandler(
			IEntityRepository<ControlFormAssign> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormAssignGetIdsResult>> ExecuteAsync(ControlFormAssignGetIdsArguments arguments)
		{
			var items = await Repository.GetAsync();
			var result = items
				.Where(x => arguments.Ids.Contains(x.Id))
				.OrderBy(x => new { x.Form.Name })
				.Select(x => new
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.Form.Name,
					Type = 
						x.Check.ItemsSince.Count() > 0 ? PresenceType.Entrance :
						x.Check.ItemsUntil.Count() > 0 ? PresenceType.Exit :
						PresenceType.Round,
					DateTime = x.Check.Date,
					PresencesCount = x.Check.ControlPresences.Count(),
					Values = x.Values
						.Select(y => new ControlFormAssignGetIdsResult_Value
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
				})
				.ToList()
				.Select(x => new ControlFormAssignGetIdsResult_Assign
				{
					Id = x.Id,
					FormId = x.FormId,
					FormName = x.FormName,
					Type = x.Type,
					DateTime = x.DateTime, // Need to be done in memory
					PresencesCount = x.PresencesCount,
					Values = x.Values
				});

			return new ResultBase<ControlFormAssignGetIdsResult>
			{
				Data = new List<ControlFormAssignGetIdsResult>
				{
					new ControlFormAssignGetIdsResult { Assigns = result }
				}
			};
		}
		#endregion ExecuteAsync
	}
}
