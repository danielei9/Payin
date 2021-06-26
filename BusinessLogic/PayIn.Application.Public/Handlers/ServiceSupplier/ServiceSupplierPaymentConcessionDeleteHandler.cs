using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceSupplierPaymentConcessionDeleteHandler :
		IServiceBaseHandler<ServiceSupplierPaymentConcessionDeleteArguments>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;
				
		#region Constructors
		public ServiceSupplierPaymentConcessionDeleteHandler
		(
			IEntityRepository<ServiceConcession> repository			
		)
		{			
			if (repository == null) throw new ArgumentNullException("repository");					
			Repository = repository;				
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceSupplierPaymentConcessionDeleteArguments arguments)
		{			
			var items = (await Repository.GetAsync())
				.Where(x => x.SupplierId == arguments.SupplierId);
			var result = items
				.Where(y => y.Id == arguments.Id)
				.FirstOrDefault();
			result.State = ConcessionState.Removed;		
		
			//var count = items
			//	.Where(z => 
			//		z.Type == result.Type && 
			//		z.SupplierId == arguments.SupplierId && 
			//		z.State == ConcessionState.Active)
			//	.Count();

			//if (count == 0) 
			//{
			//	if (result.Type == ServiceType.Charge)
			//	{
			//		//Eliminar rol commercePayment
			//	}
			//	else
			//	{ 
			//		//Eliminar rol comerce
			//	}
			//}
			return result;
		}
		#endregion ExecuteAsync
	}
}
