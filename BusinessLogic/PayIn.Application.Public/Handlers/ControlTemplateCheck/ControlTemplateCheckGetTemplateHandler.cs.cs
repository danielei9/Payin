using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCheckGetTemplateHandler :
		IQueryBaseHandler<ControlTemplateCheckGetTemplateArguments, ControlTemplateCheckGetTemplateResult>
	{
		private readonly IEntityRepository<ControlTemplateCheck> Repository;

		#region Constructors
		public ControlTemplateCheckGetTemplateHandler(IEntityRepository<ControlTemplateCheck> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateCheckGetTemplateResult>> IQueryBaseHandler<ControlTemplateCheckGetTemplateArguments, ControlTemplateCheckGetTemplateResult>.ExecuteAsync(ControlTemplateCheckGetTemplateArguments arguments)
		{
			var checks = await Repository.GetAsync();
							
			var result = checks
				.Where(x => 
					x.TemplateId == arguments.TemplateId &&
					x.ItemsSince.Count() + x.ItemsUntil.Count() == 0
				)
				.OrderBy(x => x.Id)
				.Select(x => new
				{
					Id = x.Id,
					Time = x.Time,
					FormsCount = x.FormAssignTemplates.Count(),
					CheckPoint = x.CheckPoint.Name			
				})
				.ToList()
				.Select(x => new ControlTemplateCheckGetTemplateResult
				{
					Id = x.Id,
					Time = x.Time,
					FormsCount = x.FormsCount,// Need to be done in memory
					CheckPoint = x.CheckPoint   
				});

			return new ResultBase<ControlTemplateCheckGetTemplateResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
