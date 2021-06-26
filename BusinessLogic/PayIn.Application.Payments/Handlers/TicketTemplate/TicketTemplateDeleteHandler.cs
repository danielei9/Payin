using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketTemplateDeleteHandler :
		IServiceBaseHandler<TicketTemplateDeleteArguments>
	{		
		private readonly IEntityRepository<TicketTemplate> Repository;


		#region Constructors
		public TicketTemplateDeleteHandler(IEntityRepository<TicketTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			
			Repository = repository;			
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TicketTemplateDeleteArguments>.ExecuteAsync(TicketTemplateDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

		if (item.IsGeneric == true)
			throw new Exception(TicketTemplateResources.GenericException);

		await Repository.DeleteAsync(item);


			return null;
		}
		#endregion ExecuteAsync
	}
}
