using PayIn.Application.Dto.Arguments.ControlForm;
using PayIn.Application.Dto.Results.ControlForm;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common;
using Microsoft.Practices.Unity;
using PayIn.BusinessLogic.Common;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormGetAllHandler :
		IQueryBaseHandler<ControlFormGetAllArguments, ControlFormGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PayIn.Domain.Public.ControlForm> Repository { get; set; }

		#region Constructors
		public ControlFormGetAllHandler(
			IEntityRepository<PayIn.Domain.Public.ControlForm> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ControlFormGetAllResult>> IQueryBaseHandler<ControlFormGetAllArguments, ControlFormGetAllResult>.ExecuteAsync(ControlFormGetAllArguments arguments)
		{
			var forms = (await Repository.GetAsync())
				.Where(x =>
					x.Concession.Supplier.Login == SessionData.Login ||
					x.Concession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
				);
			
			if (!arguments.Filter.IsNullOrEmpty())
				forms = forms.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Observations.Contains(arguments.Filter)
				));

			var result = forms
				.Select(x => new ControlFormGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					Observations = x.Observations,
					State = x.State,
					NumArguments = x.Arguments
						.Where(y => 
							y.State == ControlFormArgumentState.Active
						)
						.Count()
				})
				.OrderBy(x => x.Id);

			return new ResultBase<ControlFormGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
