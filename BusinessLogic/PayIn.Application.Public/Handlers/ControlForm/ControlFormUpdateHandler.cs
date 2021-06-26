using PayIn.Application.Dto.Arguments.ControlForm;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormUpdateHandler :
		IServiceBaseHandler<ControlFormUpdateArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ControlForm> Repository;
		
		#region Contructors
		public ControlFormUpdateHandler(
			IEntityRepository<PayIn.Domain.Public.ControlForm> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlFormUpdateArguments>.ExecuteAsync(ControlFormUpdateArguments arguments)
		{
			var form = await Repository.GetAsync(arguments.Id);

			form.Name = arguments.Name;
			form.Observations = arguments.Observations;
		
			return form;
		}
		#endregion ExecuteAsync
	}
}
