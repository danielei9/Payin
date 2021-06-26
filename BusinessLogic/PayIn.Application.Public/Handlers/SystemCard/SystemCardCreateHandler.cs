using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.SystemCard;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class SystemCardCreateHandler : IServiceBaseHandler<SystemCardCreateArguments>
	{
		[Dependency] public IEntityRepository<SystemCard> RepositorySystemCard { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardCreateArguments arguments)
		{
			var systemCard = new SystemCard
			{
				Name = arguments.Name,
				CardType= arguments.CardType,
				NumberGenerationType = arguments.NumberGenerationType,
				PhotoUrl = "",
				ConcessionOwnerId = arguments.ConcessionOwnerId,
				AffiliationEmailBody = arguments.AffiliationEmailBody,
				SynchronizationInterval = arguments.SynchronizationInterval,
				ClientId = arguments.ClientId
		};
			await RepositorySystemCard.AddAsync(systemCard);
			
			return systemCard;
		}
		#endregion ExecuteAsync
	}
}
