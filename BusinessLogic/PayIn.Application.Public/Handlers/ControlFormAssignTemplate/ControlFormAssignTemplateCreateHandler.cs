using PayIn.Application.Dto.Arguments.ControlFormAssignTemplate;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignTemplateCreateHandler :
		IServiceBaseHandler<ControlFormAssignTemplateCreateArguments>
	{
		private readonly IEntityRepository<ControlFormAssignTemplate> Repository;
		private readonly IEntityRepository<ControlForm> RepositoryForm;

		#region Constructors
		public ControlFormAssignTemplateCreateHandler(
			IEntityRepository<ControlFormAssignTemplate> repository,
			IEntityRepository<ControlForm> repositoryForm
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryForm == null) throw new ArgumentNullException("repositoryForm");

			Repository = repository;
			RepositoryForm = repositoryForm;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlFormAssignTemplateCreateArguments arguments)
		{
			var form = await RepositoryForm.GetAsync(arguments.FormId, new string[] { "Arguments", "AssignTemplates" });
			if (form.AssignTemplates.Any(x => x.CheckId == arguments.CheckId))
				throw new ApplicationException(ControlFormAssignTemplateResources.ExceptionFormAlreadyAssigned);

			if (form.Arguments.Count == 0)
				throw new Exception(ControlFormAssignTemplateResources.ExceptionFormNoArguments);

			var item = new ControlFormAssignTemplate {
				FormId = arguments.FormId,
				CheckId = arguments.CheckId
			};
			
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
