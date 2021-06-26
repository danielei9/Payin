using PayIn.Application.Dto.Transport.Arguments.TransportPrice;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportPriceCreateHandler : IServiceBaseHandler<TransportPriceCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportPrice> Repository;
		private readonly IEntityRepository<TransportTitle> RepositoryTransportTitle;

		#region Constructors
		public TransportPriceCreateHandler(
			ISessionData sessionData,
			IEntityRepository<TransportPrice> repository,
			IEntityRepository<TransportTitle> repositoryTransportTitle
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryTransportTitle == null) throw new ArgumentNullException("repositoryTransportTitle");

			SessionData = sessionData;
			Repository = repository;
			RepositoryTransportTitle = repositoryTransportTitle;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportPriceCreateArguments arguments)
		{
			var transportTitle = (await RepositoryTransportTitle.GetAsync())
			.Where(x => x.Id == arguments.TransportTitleId)
			.FirstOrDefault();
				

			if (transportTitle == null)
				throw new Exception(string.Format("No existe un título de transportes asociado a este precio."));

			var price = new TransportPrice
			{
				Start = arguments.Start,
				End = arguments.End,
				Version = arguments.Version,
				Price = arguments.Price,
				Zone = arguments.Zone ?? null,
				TransportTitleId = arguments.TransportTitleId,
				State = TransportPriceState.Active,
				MaxTimeChanges = arguments.MaxTimeChanges,
				OperatorContext = arguments.OperatorContext
			};
			await Repository.AddAsync(price);

			return price;
		}
		#endregion ExecuteAsync
	}
}

