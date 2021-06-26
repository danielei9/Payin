using PayIn.Application.Dto.Arguments.ControlForm;
using PayIn.Application.Dto.Results.ControlForm;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormGetSelectorHandler :
		IQueryBaseHandler<ControlFormGetSelectorArguments, ControlFormGetSelectorResult>
	{
		private readonly IEntityRepository<ControlForm> _Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ControlFormGetSelectorHandler(
			IEntityRepository<ControlForm> repository,
			ISessionData sessionData
			)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormGetSelectorResult>> ExecuteAsync(ControlFormGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Where(x => 
					x.Concession.Supplier.Login == SessionData.Login ||
					x.Concession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login) 
					)
				.Select(x => new ControlFormGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ControlFormGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
