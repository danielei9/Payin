using PayIn.Application.Dto.Transport.Arguments.TransportCardSupport;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportCardSupportUpdateHandler :
		IServiceBaseHandler<TransportCardSupportUpdateArguments>
	{
		private readonly IEntityRepository<TransportCardSupport> Repository;
		
		#region Contructors
		public TransportCardSupportUpdateHandler(
			IEntityRepository<TransportCardSupport> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TransportCardSupportUpdateArguments>.ExecuteAsync(TransportCardSupportUpdateArguments arguments)
		{
			var transportcardsupport = await Repository.GetAsync(arguments.Id);
			
			transportcardsupport.Name = arguments.Name;
			transportcardsupport.OwnerName = arguments.OwnerName;
			transportcardsupport.OwnerCode = arguments.OwnerCode;
			transportcardsupport.Type = arguments.Type;
			transportcardsupport.SubType = arguments.SubType;

			return transportcardsupport;
		}
		#endregion ExecuteAsync
	}
}
