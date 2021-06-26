using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Application.Dto.Results.ServiceSupplier;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceSupplierGetCurrentHandler :
		IQueryBaseHandler<ServiceSupplierGetCurrentArguments, ServiceSupplierGetCurrentResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ServiceSupplier> Repository;
		private IInternalService InternalService;
		
		#region Constructors
		public ServiceSupplierGetCurrentHandler(
			ISessionData sessionData,
			IEntityRepository<ServiceSupplier> repository,
			IInternalService internalService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null)	throw new ArgumentNullException("repository");
			if (internalService == null) throw new ArgumentNullException("internalService");
			
			SessionData = sessionData;
			Repository = repository;
			InternalService = internalService;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceSupplierGetCurrentResult>> IQueryBaseHandler<ServiceSupplierGetCurrentArguments, ServiceSupplierGetCurrentResult>.ExecuteAsync(ServiceSupplierGetCurrentArguments arguments)
		{
			var items = await Repository.GetAsync();
			var result = items
				.Where(x => x.Login == SessionData.Login)
				.Select(x => new ServiceSupplierGetCurrentResult
				{
					SupplierName = x.Name,
					TaxName = x.TaxName,
					TaxNumber = x.TaxNumber,
					TaxAddress = x.TaxAddress
				})
				.ToList()
				;

			if (result.Count() == 0) 
			{
				var res = new ServiceSupplierGetCurrentResult { };
				result.Add(res);
			}

			if (await InternalService.UserHasPaymentAsync())
			{
				result.FirstOrDefault().ShowPinForm = false;
				result.FirstOrDefault().Pin = ServiceSupplierResources.Pass;
				result.FirstOrDefault().PinConfirmation = ServiceSupplierResources.Pass;
			}
			else
			{
				result.FirstOrDefault().ShowPinForm = true;
			}
			return new ResultBase<ServiceSupplierGetCurrentResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
