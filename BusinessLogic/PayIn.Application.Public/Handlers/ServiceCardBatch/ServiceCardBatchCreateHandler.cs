using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardBatchCreateHandler : IServiceBaseHandler<ServiceCardBatchCreateArguments>
    {
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCardBatch> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceCardBatchCreateArguments arguments)
		{
			var systemCard = (await SystemCardRepository.GetAsync())
				.Where(x => x.Id == arguments.SystemCardId)
				.FirstOrDefault();
			if (systemCard == null)
				throw new ArgumentNullException("SystemCardId");

			var ServiceCardBatch = new ServiceCardBatch
			{
				Name = arguments.Name,
				State = Common.ServiceCardBatchState.Active,
				ConcessionId = systemCard.ConcessionOwnerId,
				SystemCard = systemCard,
				UidFormat = arguments.UidFormat
			};

			await Repository.AddAsync(ServiceCardBatch);

			return ServiceCardBatch;
		}
		#endregion ExecuteAsync
	}
}

