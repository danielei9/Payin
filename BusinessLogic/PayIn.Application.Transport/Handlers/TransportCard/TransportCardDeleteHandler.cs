using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportCardDeleteHandler : IServiceBaseHandler<TransportCardDeleteArguments>
	{
		private readonly IEntityRepository<TransportCard> Repository;

		#region Constructors
		public TransportCardDeleteHandler(
			IEntityRepository<TransportCard> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportCardDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = TransportCardState.Deleted;

			return item;
		}
		#endregion ExecuteAsync
	}
}

