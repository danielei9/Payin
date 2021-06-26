using PayIn.Application.Dto.Arguments.ControlFormAssignTemplate;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignTemplateDeleteHandler :
		IServiceBaseHandler<ControlFormAssignTemplateDeleteArguments>
	{
		private readonly IEntityRepository<ControlFormAssignTemplate> Repository;

		#region Constructors
		public ControlFormAssignTemplateDeleteHandler(
			IEntityRepository<ControlFormAssignTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormAssignTemplateDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
