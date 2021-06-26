using PayIn.Application.Dto.Transport.Arguments.TransportPrice;
using PayIn.Application.Dto.Transport.Arguments.TransportTitle;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportPriceUpdateHandler :
		IServiceBaseHandler<TransportPriceUpdateArguments>
	{
		private readonly IEntityRepository<TransportPrice> Repository;
		
		#region Contructors
		public TransportPriceUpdateHandler(
			IEntityRepository<TransportPrice> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportPriceUpdateArguments arguments)
		{
			var transportPrice = (await Repository.GetAsync(arguments.Id));
				
			transportPrice.Start = arguments.Start;
			transportPrice.End = arguments.End;
			transportPrice.Version = arguments.Version;
			transportPrice.Price = arguments.Price;			
			transportPrice.Zone = arguments.Zone;
			transportPrice.MaxTimeChanges = arguments.MaxTimeChanges;
			transportPrice.OperatorContext = arguments.OperatorContext;

			return transportPrice;
		}
		#endregion ExecuteAsync
	}
}
