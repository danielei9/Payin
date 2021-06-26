using PayIn.Application.Dto.Arguments.ControlForm;
using PayIn.Application.Dto.Results.ControlForm;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormGetHandler :
		IQueryBaseHandler<ControlFormGetArguments, ControlFormGetResult>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ControlForm> _Repository;

		#region Constructors
		public ControlFormGetHandler(
			IEntityRepository<PayIn.Domain.Public.ControlForm> repository 
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlFormGetResult>> IQueryBaseHandler<ControlFormGetArguments, ControlFormGetResult>.ExecuteAsync(ControlFormGetArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					Arguments = x.Arguments.Select(y => new
					{
						Id = y.Id,
						Name = y.Name,
						Observations = y.Observations,
						Type = y.Type,
						Target = y.Target,
					})
				})
				.ToList()
				.Select(x => new ControlFormGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					Arguments = x.Arguments.Select(y => new ControlFormGetResult.ControlFormGetResult_Arguments
					{
						Id = y.Id,
						Name = y.Name,
						Observations = y.Observations,
						Type = y.Type,
						Target = y.Target,
					})
				});
					

			return new ResultBase<ControlFormGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
