using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Application.Dto.Results.ServiceSupplier;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceSupplierUpdateHandler :
		IServiceBaseHandler<ServiceSupplierUpdateArguments>
	{
		private readonly IEntityRepository<ServiceSupplier> Repository;

		#region Constructors
		public ServiceSupplierUpdateHandler(IEntityRepository<ServiceSupplier> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceSupplierUpdateArguments>.ExecuteAsync(ServiceSupplierUpdateArguments arguments)
		{
			var items = await Repository.GetAsync(arguments.Id);

			items.Login = arguments.Login;
			items.Name = arguments.Name;
			items.TaxName = arguments.TaxName;
			items.TaxNumber = arguments.TaxNumber;
			items.TaxAddress = arguments.TaxAddress;
					

			return items;

		}
		#endregion ExecuteAsync
	}
}
