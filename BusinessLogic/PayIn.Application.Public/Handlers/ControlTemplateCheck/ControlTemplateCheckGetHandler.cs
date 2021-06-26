using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Application.Dto.Results.ControlTemplateCheck;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCheckGetHandler :
		IQueryBaseHandler<ControlTemplateCheckGetArguments, ControlTemplateCheckGetResult>
	{
		private readonly IEntityRepository<ControlTemplateCheck> _Repository;

		#region Constructors
		public ControlTemplateCheckGetHandler(IEntityRepository<ControlTemplateCheck> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlTemplateCheckGetResult>> IQueryBaseHandler<ControlTemplateCheckGetArguments, ControlTemplateCheckGetResult>.ExecuteAsync(ControlTemplateCheckGetArguments arguments)
		{
			var checks = await _Repository.GetAsync();

			var result = checks
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Time = x.Time,
					CheckPoint = x.CheckPoint.Name  
				})
				.ToList()
				.Select(x => new ControlTemplateCheckGetResult
				{
					Id = x.Id,
					Time = x.Time, // Need to be done in memory
					CheckPoint = x.CheckPoint		
				});

			return new ResultBase<ControlTemplateCheckGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
