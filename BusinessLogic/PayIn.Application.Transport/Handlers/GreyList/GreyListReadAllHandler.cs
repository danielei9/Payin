using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class GreyListReadAllHandler :
		IServiceBaseHandler<GreyListReadAllArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;
		
		#region Constructor
		public GreyListReadAllHandler(
			IEntityRepository<GreyList> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			Repository = repository;

		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(GreyListReadAllArguments arguments)
		{
			if (arguments.ReadAllCard == false)
			{
				return null;
			}
			else
			{
				var action = new GreyList
				{
					Uid = arguments.Id,
					RegistrationDate = DateTime.Now.ToUTC(),
					Action = GreyList.ActionType.ReadAll,
					Field = null,
					NewValue = null,
					Resolved = false,
					ResolutionDate = null,
					Machine = GreyList.MachineType.All,
					Source = GreyList.GreyListSourceType.Payin,
					State = GreyList.GreyListStateType.Active
				};
				await Repository.AddAsync(action);
			}
			return null;
		}
		#endregion ExecuteAsync
	}
}
