using PayIn.Application.Dto.Arguments.ControlTemplateItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateItemDeleteHandler :
			IServiceBaseHandler<ControlTemplateItemDeleteArguments>
	{
		private readonly IEntityRepository<ControlTemplateItem> Repository;
		private readonly IEntityRepository<ControlTemplateCheck> RepositoryControlTemplateCheck;

		#region Constructors
		public ControlTemplateItemDeleteHandler(
			IEntityRepository<ControlTemplateItem> repository,
			IEntityRepository<ControlTemplateCheck> repositoryControlTemplateCheck
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryControlTemplateCheck == null) throw new ArgumentNullException("repositoryControlTemplateCheck");

			Repository = repository;
			RepositoryControlTemplateCheck = repositoryControlTemplateCheck;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateItemDeleteArguments>.ExecuteAsync(ControlTemplateItemDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id, "Since", "Until");

			await RepositoryControlTemplateCheck.DeleteAsync(item.Since);
			await RepositoryControlTemplateCheck.DeleteAsync(item.Until);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ExecuteAsync
	}
}
