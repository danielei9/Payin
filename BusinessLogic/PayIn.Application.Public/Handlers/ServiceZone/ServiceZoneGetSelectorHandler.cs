using PayIn.Application.Dto.Arguments.ServiceZone;
using PayIn.Application.Dto.Results.ServiceZone;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceZoneGetSelectorHandler :
		IQueryBaseHandler<ServiceZoneGetSelectorArguments, ServiceZoneGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceZone> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public ServiceZoneGetSelectorHandler(IEntityRepository<ServiceZone> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceZoneGetSelectorResult>> IQueryBaseHandler<ServiceZoneGetSelectorArguments, ServiceZoneGetSelectorResult>.ExecuteAsync(ServiceZoneGetSelectorArguments arguments)
		{
			arguments.Filter = (arguments.Filter ?? "").ToLower();

			var items = await _Repository.GetAsync();

			var result = items
				.Where(x =>
					x.Name.Contains(arguments.Filter)
				)
				.Select(x => new ServiceZoneGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceZoneGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
