using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeFormGetHandler :
		IQueryBaseHandler<EntranceTypeFormGetArguments, EntranceTypeFormGetResult>
	{
		private readonly IEntityRepository<EntranceTypeForm> Repository;

		#region Constructors
		public EntranceTypeFormGetHandler(
			IEntityRepository<EntranceTypeForm> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeFormGetResult>> ExecuteAsync(EntranceTypeFormGetArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new
				{
					Id = x.Id,
					Order = x.Order,
					EntranceTypeId = x.EntranceTypeId,
					FormId = x.FormId
				})
				.Select(x => new EntranceTypeFormGetResult
				{
					Id = x.Id,
					Order = x.Order,
					EntranceTypeId = x.EntranceTypeId,
					FormId = x.FormId
				});
			return new ResultBase<EntranceTypeFormGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
