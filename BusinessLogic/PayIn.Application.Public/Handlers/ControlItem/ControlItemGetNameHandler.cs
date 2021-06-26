using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemGetNameHandler :
		IQueryBaseHandler<ControlItemGetNameArguments, ControlItemGetNameResult>
	{
		private readonly IEntityRepository<ControlItem> _Repository;

		#region Constructors
		public ControlItemGetNameHandler(IEntityRepository<ControlItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ControlItemGet
		async Task<ResultBase<ControlItemGetNameResult>> IQueryBaseHandler<ControlItemGetNameArguments, ControlItemGetNameResult>.ExecuteAsync(ControlItemGetNameArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new ControlItemGetNameResult
				{
					Id = x.Id,
					Name = x.Name
				});

			return new ResultBase<ControlItemGetNameResult> { Data = result };
		}
		#endregion ControlItemGet
	}
}
