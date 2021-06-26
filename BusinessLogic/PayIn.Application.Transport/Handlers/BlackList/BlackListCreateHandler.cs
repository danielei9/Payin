using PayIn.Application.Dto.Transport.Arguments.BlackList;
using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class BlackListCreateHandler :
		IServiceBaseHandler<BlackListCreateArguments>
	{
		private readonly IEntityRepository<BlackList> Repository;

		#region Constructor
		public BlackListCreateHandler(
			IEntityRepository<BlackList> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;

		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(BlackListCreateArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			var create = new BlackList
			{
				Uid = arguments.Id,
				RegistrationDate = now,
				Machine = BlackListMachineType.All,					
				Resolved = false,
				ResolutionDate = null,
				Concession = 0,
				Service = BlackListServiceType.Rejection,
				IsConfirmed = false,
				Source = PayIn.Domain.Transport.BlackList.BlackListSourceType.Payin,
				State = BlackList.BlackListStateType.Active
			};
			await Repository.AddAsync(create);

			return create;
		}
		#endregion ExecuteAsync
	}
}
