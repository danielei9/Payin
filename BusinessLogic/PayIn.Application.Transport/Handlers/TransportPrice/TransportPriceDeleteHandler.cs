using PayIn.Application.Dto.Transport.Arguments.TransportPrice;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	class TransportPriceDeleteHandler : IServiceBaseHandler<TransportPriceDeleteArguments>
	{
		private readonly IEntityRepository<TransportPrice> Repository;

		#region Constructors
		public TransportPriceDeleteHandler(
			IEntityRepository<TransportPrice> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportPriceDeleteArguments>.ExecuteAsync(TransportPriceDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = TransportPriceState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}

