using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormOptionCreateHandler : 
		IServiceBaseHandler<ControlFormOptionCreateArguments>
	{
		private readonly IUnitOfWork UnitOfWork;
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<ControlFormOption> Repository;

		#region Constructors
		public ControlFormOptionCreateHandler(
			IUnitOfWork unitOfWork,
			ISessionData sessionData,
			IEntityRepository<ControlFormOption> repository
			)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			UnitOfWork = unitOfWork;
			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormOptionCreateArguments arguments)
		{
			var option = new ControlFormOption
			{
				Text = arguments.Text,
				Value = arguments.Value,
				ArgumentId = arguments.ArgumentId,
				State = ControlFormOptionState.Active
			};
			await Repository.AddAsync(option);
			return option;
		}
		#endregion ExecuteAsync
	}
}
