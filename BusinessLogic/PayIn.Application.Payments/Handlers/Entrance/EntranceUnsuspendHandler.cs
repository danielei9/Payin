using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class EntranceUnsuspendHandler :
		IServiceBaseHandler<EntranceUnsuspendArguments>
	{
		private readonly IEntityRepository<Entrance> Repository;

		#region Constructors
		public EntranceUnsuspendHandler(IEntityRepository<Entrance> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceUnsuspendArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = EntranceState.Active;

			return item;
		}
		#endregion ExecuteAsync
	}
}
