using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Application.Dto.Transport.Results.TransportCardSupport;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardSupportGetAllHandler : IQueryBaseHandler<TransportCardSupportGetAllArguments, TransportCardSupportGetAllResult>
	{

		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportCardSupport> Repository;


		#region Constructors
		public TransportCardSupportGetAllHandler(
			ISessionData sessionData,
			IEntityRepository<TransportCardSupport> repository)

		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			SessionData = sessionData;
			Repository = repository;

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<TransportCardSupportGetAllResult>> IQueryBaseHandler<TransportCardSupportGetAllArguments, TransportCardSupportGetAllResult>.ExecuteAsync(TransportCardSupportGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
				x.OwnerCode == arguments.OwnerId);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.OwnerName.Contains(arguments.Filter)
				));

			
			var supportCards = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					OwnerCode = x.OwnerCode,
					OwnerName = x.OwnerName,
					Type = x.Type,
					SubType = x.SubType,
					State = (int)x.State
				})
				.ToList()
				.Select(x => new TransportCardSupportGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					OwnerCode = x.OwnerCode,
					OwnerName = x.OwnerName,
					Type = x.Type,
					SubType = x.SubType,
					State = x.State
				})
				.OrderBy(x => x.Id);

			return new ResultBase<TransportCardSupportGetAllResult> { Data = supportCards };

		}
		#endregion ExecuteAsync
	}
}

