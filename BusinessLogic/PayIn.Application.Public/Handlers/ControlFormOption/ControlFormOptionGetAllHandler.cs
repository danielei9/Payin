using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	class ControlFormOptionGetAllHandler :
		IQueryBaseHandler<ControlFormOptionGetAllArguments, ControlFormOptionGetAllResult>
	{
		private readonly IUnitOfWork UnitOfWork;
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlFormOption> Repository;

		#region Constructors
		public ControlFormOptionGetAllHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IEntityRepository<ControlFormOption> repository
			)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ControlFormOptionGetAllResult>> ExecuteAsync (ControlFormOptionGetAllArguments arguments)
		{
			var options = (await Repository.GetAsync())
				.Where(x =>
				x.ArgumentId == arguments.ArgumentId
				);

			//if (arguments.Filter != null)
			//	options = options
			//		.Where(x =>
			//			!x.Text.Contains(arguments.Filter)
			//		);

			var result = options
				.Select(x => new
				{
					Id			= x.Id,
					Text		= x.Text,
					Value		= x.Value,
					ArgumentId	= x.ArgumentId,
					State		= x.State
				})
				.ToList()
				.Select(x => new ControlFormOptionGetAllResult
				{
					Id			= x.Id,
					Text		= x.Text,
					Value		= x.Value,
					ArgumentId	= x.ArgumentId,
					State		= x.State
				});
			return new ResultBase<ControlFormOptionGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}