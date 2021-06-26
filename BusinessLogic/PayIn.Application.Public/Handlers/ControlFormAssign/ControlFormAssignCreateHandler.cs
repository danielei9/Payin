using PayIn.Application.Dto.Arguments.ControlFormAssign;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignCreateHandler :
		IServiceBaseHandler<ControlFormAssignCreateArguments>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;
		private readonly IEntityRepository<ControlForm> RepositoryForm;

		#region Constructors
		public ControlFormAssignCreateHandler(
			IEntityRepository<ControlFormAssign> repository,
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
		public async Task<dynamic> ExecuteAsync(ControlFormAssignCreateArguments arguments)
		{
			var form = await RepositoryForm.GetAsync(arguments.FormId, "Arguments", "Assigns");
			if (form.Assigns.Any(x => x.CheckId == arguments.CheckId))
				throw new ApplicationException(ControlFormAssignResources.ExceptionFormAlreadyAssigned);

			var item = new ControlFormAssign {
				Form = form,
				CheckId = arguments.CheckId
			};
			await Repository.AddAsync(item);

			foreach (var argument in form.Arguments)
			{
				var value = new ControlFormValue
				{
					ArgumentId = argument.Id,
					Observations = argument.Observations,
					Target = argument.Target,
				};
				item.Values.Add(value);
			}

			return item;
		}
		#endregion ExecuteAsync
	}
}
