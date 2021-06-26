using PayIn.Application.Dto.Transport.Arguments.TransportSystem;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	
	public class TransportSystemCreateHandler :
		IServiceBaseHandler<TransportSystemCreateArguments>
	{
		private readonly IEntityRepository<TransportSystem> Repository;

		#region Contructors
		public TransportSystemCreateHandler(IEntityRepository<TransportSystem> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");			

			Repository = repository;		
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportSystemCreateArguments arguments)
		{
			var system = new TransportSystem
			{
				Name = arguments.Name,
				CardType = arguments.CardType,				
				State = TransportSystemState.Active				
			};
			await Repository.AddAsync(system);
			return system;
		}
		#endregion ExecuteAsync
	}
}
