using PayIn.Application.Dto.Arguments.ControlForm;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormCreateHandler :
		IServiceBaseHandler<ControlFormCreateArguments>
	{
		private readonly IEntityRepository<ControlForm> Repository;

		#region Constructors
		public ControlFormCreateHandler(
			IEntityRepository<ControlForm> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlFormCreateArguments>.ExecuteAsync(ControlFormCreateArguments arguments)
		{
			var item = new ControlForm
			{
				Name = arguments.Name ?? "",
				Observations = arguments.Observations ?? "",
				ConcessionId = arguments.ConcessionId,
				State = Common.ControlFormState.Active
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
