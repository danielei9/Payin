using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class GreyListCreateHandler :
		IServiceBaseHandler<GreyListCreateArguments>
	{
		private readonly IEntityRepository<GreyList> Repository;

		#region Constructor
		public GreyListCreateHandler(
			IEntityRepository<GreyList> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			Repository = repository;

		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(GreyListCreateArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

				var create = new GreyList
				{
					Uid = arguments.Id,
					RegistrationDate = now,
					Action = PayIn.Domain.Transport.GreyList.ActionType.ModifyField,
					Field = arguments.Key,
					NewValue = arguments.Value,
					Resolved = false,
					ResolutionDate = null,
					Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
					Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
					State = GreyList.GreyListStateType.Active
				};
				await Repository.AddAsync(create);

			return create;
		}
		#endregion ExecuteAsync
	}
}
