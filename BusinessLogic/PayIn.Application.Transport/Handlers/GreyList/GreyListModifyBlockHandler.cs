using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Common.Resources;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class GreyListModifyBlockHandler :
		IServiceBaseHandler<GreyListModifyBlockArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;
		private readonly IEntityRepository<TransportOperation> TransportRepository;
		
		#region Constructor
		public GreyListModifyBlockHandler(
			IEntityRepository<GreyList> repository,
			IEntityRepository<TransportOperation> transportRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			if (transportRepository == null) throw new ArgumentNullException("TransportRepository");

			Repository = repository;
			TransportRepository = transportRepository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(GreyListModifyBlockArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			var operation = (await TransportRepository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.OrderByDescending(x => x.Id)
				.FirstOrDefault();

			var greylist = (await Repository.GetAsync())
				.Where(x => x.Uid == operation.Uid)
				.OrderByDescending(x => x.Id)
				.FirstOrDefault();

			if (operation == null || operation.Uid == null)
				throw new Exception(GreyListResources.IdNotFound);

			foreach (var item in arguments.Items){

				var count = 0;
				var exit = false;
				for ( int i=0;i<15; i++)
				{
					for (int j=0;j<4;j++)
					{
						if (i == Convert.ToInt32(item.sector) && j == Convert.ToInt32(item.block))
						{
							exit = true;
							break;							
						}
						else
							count++;
					}
					if (exit == true)
						break;
				}

				var action = new GreyList
				{
					Uid = (long)operation.Uid,
					RegistrationDate = now,
					Action = PayIn.Domain.Transport.GreyList.ActionType.ModifyBlock,
					Field = (count > 9) ? "BLO" + count.ToIsoString() : "BLO0" + count.ToIsoString(),
					NewValue = item.quantity,
					Resolved = false,
					ResolutionDate = null,
					Machine = GreyList.MachineType.All,
					Source = GreyList.GreyListSourceType.Payin,
					OperationNumber = (greylist == null) ? 0 : greylist.OperationNumber + 1,
					State = GreyList.GreyListStateType.Active
				};
				await Repository.AddAsync(action);
			}		
			return null;
		}
		#endregion ExecuteAsync
	}
}
