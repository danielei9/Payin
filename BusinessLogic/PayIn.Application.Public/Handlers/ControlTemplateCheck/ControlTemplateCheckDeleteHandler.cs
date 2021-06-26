using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCheckDeleteHandler:
			IServiceBaseHandler<ControlTemplateCheckDeleteArguments>
	{
		private readonly IEntityRepository<ControlTemplateCheck> Repository;

		#region Constructors
		public ControlTemplateCheckDeleteHandler(IEntityRepository<ControlTemplateCheck> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlTemplateCheckDeleteArguments arguments)
		{
			var check = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(check);

			return null;
		}
		#endregion ExecuteAsync
	}
}
