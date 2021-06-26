using PayIn.Application.Dto.Transport.Arguments.GreyList;
using PayIn.Application.Dto.Transport.Results.GreyList;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class GreyListGetAllHandler :
		IQueryBaseHandler<GreyListGetAllArguments, GreyListGetAllResult>
	{
		private readonly IEntityRepository<GreyList> Repository;

		#region Constructor
		public GreyListGetAllHandler(
			IEntityRepository<GreyList> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("GreyList");
			Repository = repository;			
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<GreyListGetAllResult>> ExecuteAsync(GreyListGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => x.Uid.ToString().Contains(arguments.Filter));

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Uid = x.Uid,
					RegistrationDate = x.RegistrationDate,
					Action = x.Action,
					Field = x.Field,
					NewValue = x.NewValue,
					Machine = x.Machine,
					Resolved = x.Resolved,
					ResolutionDate = x.ResolutionDate,
					Source = x.Source,
					OperationNumber = x.OperationNumber
				})
					.ToList()
					.OrderByDescending(x=>x.RegistrationDate)
					.Select(x => new GreyListGetAllResult
					{
						Id = x.Id,
						Uid = x.Uid,
						RegistrationDate = x.RegistrationDate,
						Action = x.Action,
						ActionAlias = x.Action.ToEnumAlias(),
						Field = x.Field,
						NewValue = x.NewValue,
						Machine = x.Machine,
						MachineAlias = x.Machine.ToEnumAlias(),
						Resolved = x.Resolved,
						ResolutionDate = x.ResolutionDate,
						Source = x.Source,
						SourceAlias = x.Source.ToEnumAlias(),
						OperationNumber = x.OperationNumber					
					});

			return new ResultBase<GreyListGetAllResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}
