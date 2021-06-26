using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Application.Dto.Transport.Results.TransportCardSupport;
using PayIn.Domain.Transport;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportGetHandler :
		IQueryBaseHandler<TransportCardSupportGetArguments, TransportCardSupportGetResult>
	{
		private readonly IEntityRepository<TransportCardSupport> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public TransportCardSupportGetHandler(IEntityRepository<TransportCardSupport> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardSupportGetResult>> ExecuteAsync(TransportCardSupportGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
			.Where(x => x.Id == arguments.Id)			
			.Select(x => new
			{
				Id = x.Id,
				Name = x.Name,				
				OwnerName = x.OwnerName,
				OwnerCode = x.OwnerCode,
				Type = x.Type,
				SubType = x.SubType
			})
			.Select(x => new TransportCardSupportGetResult
			{
				Id = x.Id,
				Name = x.Name,
				OwnerName = x.OwnerName,
				OwnerCode = x.OwnerCode,
				Type = x.Type,
				SubType = x.SubType
			});

			return new ResultBase<TransportCardSupportGetResult> { Data = item };
		}
		#endregion ExecuteAsync
	}
}
