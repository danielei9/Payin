using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Common.Resources;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class GreyListModifyFieldHandler :
		IServiceBaseHandler<GreyListModifyFieldArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;
		private readonly IEntityRepository<TransportOperation> TransportRepository;

		#region Constructor
		public GreyListModifyFieldHandler(
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
		public async Task<dynamic> ExecuteAsync(GreyListModifyFieldArguments arguments)
		{
			var now = DateTime.Now.ToUTC();	

			//En caso de no modificar nada
			if (arguments.ModifyBlock == null && arguments.ModifyValues == null)
				return 0;

			if (arguments.Id == 0)
				throw new ApplicationException(GreyListResources.IdNotFound);

			var operationNumber = (await Repository.GetAsync())
				.Where(x =>
					x.Uid == arguments.Uid
				)
				.Max(x => (int?)x.OperationNumber) ?? 0;

            if (arguments.ModifyValues != null && arguments.ModifyValues.Count() > 0)
			{
				foreach (var item in arguments.ModifyValues)
				{					
					var action = new GreyList
					{
						Uid = arguments.Uid,
						RegistrationDate = now,
						Action = GreyList.ActionType.ModifyField,
						Field = item.Key,
						NewValue = item.Value,
						Resolved = false,
						ResolutionDate = null,
						Machine = GreyList.MachineType.All,
						Source = GreyList.GreyListSourceType.Payin,
						OperationNumber = operationNumber + 1,
						State = GreyList.GreyListStateType.Active
					};

					await Repository.AddAsync(action);
				}
			}
			
			if (arguments.ModifyCard)
			{
				var j = 0;
				if(arguments.ModifyBlock.Length != 64 && arguments.ModifyBlock.Length != 48)				
					throw new ApplicationException(GreyListResources.CardFormatException);

				for (int i = 0; i < arguments.ModifyBlock.Length; i++)
				{					
					if (arguments.ModifyBlock.Length == 64)
					{					
						if ((i % 4 == 3) || (i == 0))
							continue;							
						j++;
					}
					else j = i;

					if (arguments.ModifyBlock[i] == "" || arguments.ModifyBlock[i] == null || arguments.ModifyBlock[i] == arguments.Block[j])
						continue;

					var action = new GreyList
						{
							Uid = arguments.Uid,
							RegistrationDate = now,
							Action = GreyList.ActionType.ModifyBlock,
							Field = (i < 10) ? "BLO0" + i : "BLO" + i,
							NewValue = arguments.ModifyBlock[i],
							Resolved = false,
							ResolutionDate = null,
							Machine = GreyList.MachineType.Read,
							Source = GreyList.GreyListSourceType.Payin,
							OperationNumber = operationNumber + 1,
							State = GreyList.GreyListStateType.Active
						};
						await Repository.AddAsync(action);					
				 }	
			}

			return (arguments.ModifyValues.Count() > 0) ? arguments.ModifyValues.Count() : arguments.ModifyBlock.Count();
		}
		#endregion ExecuteAsync
	}
}
