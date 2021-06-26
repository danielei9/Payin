using PayIn.Application.Dto.Arguments.ServiceNumberPlate;
using PayIn.Application.Dto.Results.ServiceNumberPlate;
using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.ServiceNumberPlate
{
	public class ServiceNumberPlateGetByUserHandler :
		IQueryBaseHandler<ServiceNumberPlateGetAllArguments, ServiceNumberPlateGetAllResult>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public ServiceNumberPlateGetByUserHandler(IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> repository, ISessionData sessionData)
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
		async Task<ResultBase<ServiceNumberPlateGetAllResult>> IQueryBaseHandler<ServiceNumberPlateGetAllArguments, ServiceNumberPlateGetAllResult>.ExecuteAsync(ServiceNumberPlateGetAllArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Select(x => new ServiceNumberPlateGetAllResult
				{
					Id = x.Id,
					Color = x.Color,
					Model = x.Model,
					NumberPlate = x.NumberPlate
				});

			return new ResultBase<ServiceNumberPlateGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
