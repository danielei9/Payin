using PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlFormAssignCheckPointCreateHandler :
		IServiceBaseHandler<ControlFormAssignCheckPointCreateArguments>
	{
		private readonly IEntityRepository<ControlFormAssign> Repository;
		private readonly IEntityRepository<ControlForm> RepositoryForm;

		#region Constructors
		public ControlFormAssignCheckPointCreateHandler(
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
		public async Task<dynamic> ExecuteAsync(ControlFormAssignCheckPointCreateArguments arguments)
		{
			var form = await RepositoryForm.GetAsync(arguments.FormId, new string[] { "Arguments", "Assigns" });     
			if (form.Assigns.Any(x => x.CheckPointId == arguments.CheckId))  
				throw new ApplicationException(ControlFormAssignCheckPointResources.ExceptionFormAlreadyAssigned);

			if (form.Arguments.Count == 0)
				throw new Exception(ControlFormAssignCheckPointResources.ExceptionFormNoArguments);

			var item = new ControlFormAssign
			{
				FormId = arguments.FormId,
				CheckPointId = arguments.CheckId
			};

			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
