using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionGetStateHandler :
		IServiceBaseHandler<ServiceConcessionGetStateArguments>
	{
		//Falta Testear la clase
		
		private readonly IEntityRepository<ServiceConcession> Repository;
	
		#region Constructor
		public ServiceConcessionGetStateHandler(IEntityRepository<ServiceConcession> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}	
		#endregion Constructor
	
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceConcessionGetStateArguments>.ExecuteAsync(ServiceConcessionGetStateArguments argument) 
		{
			var item = await Repository.GetAsync();
			
			item = item
				.Where(x => x.Id == argument.Id);

			return item;
		}

		#endregion ExecuteAsync

	}
}
